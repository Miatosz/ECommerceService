using ECommerceService.Api.Data;
using ECommerceService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceService.Api.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(OrderItem item)
        {
            await _context.OrderItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public IQueryable<OrderItem?> Query()
        {
            return _context.OrderItems.AsNoTracking();
        }
    }
}
