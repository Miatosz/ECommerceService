using ECommerceService.Api.Models;

namespace ECommerceService.Api.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        IQueryable<Order?> Query();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);

    }
}
