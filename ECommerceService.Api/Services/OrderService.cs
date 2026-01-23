using AutoMapper;
using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;
using ECommerceService.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerceService.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repo, IProductService srv, IMapper mapper)
        {
            _productService = srv;
            _repo = repo;
            _mapper = mapper;
        }

        public async Task CancelOrder(int orderId)
        {
            var IsOrderSent = await CheckIfOrderIsSent(orderId);
            if (IsOrderSent) throw new BadHttpRequestException("Cannot cancel sent order");
        }

        public async Task ChangeOrderStatusAsync(int id, UpdateOrderDto orderDto)
        {
            var order = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();

            var IsOrderSent = await CheckIfOrderIsSent(id);
            if (IsOrderSent) throw new BadHttpRequestException("Cannot cancel sent order");            

            if (order.OrderStatus == Models.Status.Sent)
            {
                throw new BadHttpRequestException("Cannot change status from sent");
            }

            order.ChangeStatus(orderDto.OrderStatus);
            await _repo.UpdateAsync(order);
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto dto)
        {
            if (!dto.Items.Any())
                throw new InvalidOperationException("Order must contain items");

            var order = Order.Create(dto.UserId);

            var prices = await _productService.GetPricesAsync(
                dto.Items.Select(i => i.ProductId));


            foreach (var item in dto.Items)
            {
                order.AddItem(
                    item.ProductId,
                    item.Quantity,
                    prices[item.ProductId]);
            }

            await _repo.AddAsync(order);

            return order.Id;
        }

        public async Task<List<GetOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _repo.Query().ToListAsync();

            return _mapper.Map<List<GetOrderDto>>(orders);
        }

        public async Task<GetOrderDto> GetOrderById(int id)
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Order with given id not found.");

            return _mapper.Map<GetOrderDto>(order);
        }

        public Task SendOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> CheckIfOrderIsSent(int id)
        {
            var order = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException(); 

            if (order?.OrderStatus == Status.Sent)
                return true;

            return false;
        }
    }
}
