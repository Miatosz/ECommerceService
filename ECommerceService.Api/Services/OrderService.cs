using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;
using ECommerceService.Api.Repositories;

namespace ECommerceService.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository repo, IProductService srv)
        {
            _productService = srv;
            _repo = repo;
        }

        public async Task CancelOrder(int orderId)
        {
            var IsOrderSent = await CheckIfOrderIsSent(orderId);
            if (IsOrderSent) throw new BadHttpRequestException("Cannot cancel sent order");
        }

        public async Task ChangeOrderStatus(UpdateOrderDto orderDto)
        {
            var order = await _repo.GetByIdAsync(orderDto.Id) ?? throw new KeyNotFoundException();

            var IsOrderSent = await CheckIfOrderIsSent(order.Id);
            if (IsOrderSent) throw new BadHttpRequestException("Cannot cancel sent order");            

            if (order.OrderStatus == Models.Status.Sent)
            {
                throw new BadHttpRequestException("Cannot change status from sent");
            }

            order.ChangeStatus(orderDto.OrderStatus);
            await _repo.UpdateAsync(order);
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto dto, int userId)
        {
            if (!dto.Items.Any())
                throw new InvalidOperationException("Order must contain items");

            var order = Order.Create(userId);

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
