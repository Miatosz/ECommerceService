using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;

namespace ECommerceService.Api.Services
{
    public interface IOrderService
    {
        Task<CreateOrderDto> CreateOrderAsync(CreateOrderDto order);
        Task ChangeOrderStatus(UpdateOrderDto order);

    }
}
