using ECommerceService.Api.Models;

namespace ECommerceService.Api.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetByIdAsync(int id);
        IQueryable<OrderItem?> Query();
        Task AddAsync(OrderItem orderItem);
    }
}
