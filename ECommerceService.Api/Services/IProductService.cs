using ECommerceService.Api.Dto;

namespace ECommerceService.Api.Services
{
    public interface IProductService
    {
        Task<int> CreateAsync(CreateProductDto dto);
        Task<GetProductDto> GetByIdAsync(int id);
        Task<Dictionary<int, decimal>> GetPricesAsync(IEnumerable<int> productIds);
        Task<List<GetProductDto>> GetAllProductsAsync();
        IQueryable<GetProductDto> QueryProducts();
        Task<GetProductDto> UpdateProduct(UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int id);
        Task EnsureSufficientStockAsync(int productId, int requiredQuantity);

    }
}
