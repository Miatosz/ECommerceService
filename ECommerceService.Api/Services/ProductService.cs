using AutoMapper;
using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;
using ECommerceService.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerceService.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            await _repo.AddAsync(product);

            return product.Id;
        }

        public async Task<GetProductDto> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product with given id not found.");

            return _mapper.Map<GetProductDto>(product);
        }

        public async Task<Dictionary<int, decimal>> GetPricesAsync(IEnumerable<int> productIds)
        {
            if (!productIds.Any())
                return new Dictionary<int, decimal>();

            var products = await _repo.Query()
                .Where(x => productIds.Contains(x.Id))
                .ToListAsync();

            var dict = products.ToDictionary(x => x.Id, x => x.Price);

            return dict;
        }
    }
}
