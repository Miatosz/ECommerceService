using ECommerceService.Api.Models;

namespace ECommerceService.Api.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        IQueryable<Product> Query();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<int?> GetStockByIdAsync(int id);
        Task<bool> TryDecreaseStockAsync(int productId, int quantity);
    }
}
