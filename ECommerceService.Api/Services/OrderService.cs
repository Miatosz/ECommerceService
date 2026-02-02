using AutoMapper;
using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;
using ECommerceService.Api.Repositories;
using ECommerceService.Api.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ECommerceService.Api.Services
{
    /// <summary>
    /// Serwis zarządzania zamówieniami z obsługą rezerwacji stocku i state machine dla statusów.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IStockReservationService _stockReservation;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository repo,
            IStockReservationService stockReservation,
            IProductService productService,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            _repo = repo;
            _stockReservation = stockReservation;
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Tworzy nowe zamówienie z rezerwacją atomową stocku.
        /// </summary>
        public async Task<int> CreateOrderAsync(CreateOrderDto dto)
        {
            if (!dto.Items.Any())
                throw new InvalidOrderStateException("Order must contain at least one item.");

            var reservations = new List<(int productId, int quantity)>();

            try
            {
                var priceMap = await _productService.GetPricesAsync(dto.Items.Select(i => i.ProductId));
                foreach (var item in dto.Items)
                {
                    await _stockReservation.ReserveAsync(item.ProductId, item.Quantity);
                    reservations.Add((item.ProductId, item.Quantity));
                }

                var order = Order.Create(dto.UserId);

                foreach (var item in dto.Items)
                {
                    if (!priceMap.ContainsKey(item.ProductId))
                        throw new ProductNotFoundException(item.ProductId);

                    order.AddItem(item.ProductId, item.Quantity, priceMap[item.ProductId]);
                }

                await _repo.AddAsync(order);

                _logger.LogInformation(
                    "Order created successfully: OrderId={OrderId}, UserId={UserId}, ItemCount={ItemCount}, Total={Total}",
                    order.Id, dto.UserId, order.OrderItems.Count, order.Total);

                return order.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Order creation failed. Rolling back {ReservationCount} stock reservations.",
                    reservations.Count);

                foreach (var (productId, quantity) in reservations)
                {
                    try
                    {
                        await _stockReservation.RestoreAsync(productId, quantity);
                    }
                    catch (Exception restoreEx)
                    {
                        _logger.LogError(restoreEx,
                            "Failed to restore stock for ProductId={ProductId}, Quantity={Quantity}. Manual intervention required.",
                            productId, quantity);
                    }
                }

                throw;
            }
        }

        /// <summary>
        /// Zmienia status zamówienia.
        /// </summary>
        public async Task ChangeOrderStatusAsync(int id, UpdateOrderDto orderDto)
        {
            var order = await _repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

            try
            {
                order.ChangeStatus(orderDto.OrderStatus);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOrderStateException(ex.Message);
            }

            await _repo.UpdateAsync(order);

            _logger.LogInformation(
                "Order status changed: OrderId={OrderId}, NewStatus={Status}",
                id, orderDto.OrderStatus);
        }

        /// <summary>
        /// Anuluje zamówienie i przywraca zarezerwowany stock.
        /// </summary>
        public async Task CancelOrder(int orderId)
        {
            var order = await _repo.GetByIdAsync(orderId)
                ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            if (order.OrderStatus == Status.Sent)
                throw new OrderNotModifiableException(orderId);

            foreach (var item in order.OrderItems)
            {
                await _stockReservation.RestoreAsync(item.ProductId, item.Quantity);
            }

            order.ChangeStatus(Status.Cancelled);
            await _repo.UpdateAsync(order);

            _logger.LogInformation(
                "Order cancelled: OrderId={OrderId}, RestoredItems={ItemCount}",
                orderId, order.OrderItems.Count);
        }

        /// <summary>
        /// Wysyła zamówienie (zmienia status na Sent).
        /// </summary>
        public async Task SendOrder(int orderId)
        {
            var order = await _repo.GetByIdAsync(orderId)
                ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            if (order.OrderStatus != Status.Paid)
                throw new InvalidOrderStateException(
                    $"Only paid orders can be sent. Current status: {order.OrderStatus}");

            order.ChangeStatus(Status.Sent);
            await _repo.UpdateAsync(order);

            _logger.LogInformation("Order sent: OrderId={OrderId}", orderId);
        }

        /// <summary>
        /// Pobiera wszystkie zamówienia.
        /// </summary>
        public async Task<List<GetOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _repo.Query().ToListAsync();
            return _mapper.Map<List<GetOrderDto>>(orders);
        }

        /// <summary>
        /// Pobiera zamówienie po ID z wszystkimi pozycjami.
        /// </summary>
        public async Task<GetOrderDto> GetOrderById(int id)
        {
            var order = await _repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

            return _mapper.Map<GetOrderDto>(order);
        }
    }
}
