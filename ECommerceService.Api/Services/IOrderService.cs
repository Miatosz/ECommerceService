using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;

namespace ECommerceService.Api.Services
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderDto dto);
        Task ChangeOrderStatusAsync(int id, UpdateOrderDto order);
        public Task CancelOrder(int orderId);
        public Task SendOrder(int orderId);
        public Task<List<GetOrderDto>> GetAllOrdersAsync();
        public Task<GetOrderDto> GetOrderById(int id);

    }
}
