using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;

namespace ECommerceService.Api.Services
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderDto dto, int userId);
        Task ChangeOrderStatus(UpdateOrderDto order);
        public Task CancelOrder(int orderId);
        public Task SendOrder(int orderId);

    }
}
