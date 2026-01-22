using ECommerceService.Api.Dto;
using ECommerceService.Api.Repositories;

namespace ECommerceService.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }
        public async Task ChangeOrderStatus(UpdateOrderDto orderDto)
        {
            var order = await _repo.GetByIdAsync(orderDto.Id) ?? throw new KeyNotFoundException();

            if (order.OrderStatus == Models.Status.Sent)
            {
                throw new BadHttpRequestException("Cannot change status from sent");
            }

            order.ChangeStatus(orderDto.OrderStatus);
            await _repo.UpdateAsync(order);
        }

        public Task<CreateOrderDto> CreateOrderAsync(CreateOrderDto order)
        {
            throw new NotImplementedException();
        }
    }
}
