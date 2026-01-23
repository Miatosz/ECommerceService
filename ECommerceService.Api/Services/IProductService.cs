using ECommerceService.Api.Dto;

namespace ECommerceService.Api.Services
{
    public interface IProductService
    {
        Task<int> CreateAsync(CreateProductDto dto);
        Task<GetProductDto> GetByIdAsync(int id);
        Task<Dictionary<int, decimal>> GetPricesAsync(IEnumerable<int> productIds);

    }
}
