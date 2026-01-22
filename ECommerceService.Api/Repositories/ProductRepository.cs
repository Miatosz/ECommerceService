using ECommerceService.Api.Data;
using ECommerceService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceService.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public IQueryable<Product?> Query()
        {
            return _context.Products.AsNoTracking();
        }
    }
}
