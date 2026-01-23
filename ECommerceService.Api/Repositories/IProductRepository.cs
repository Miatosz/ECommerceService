using ECommerceService.Api.Models;

namespace ECommerceService.Api.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        IQueryable<Product?> Query();
        Task AddAsync(Product product);

    }
}
