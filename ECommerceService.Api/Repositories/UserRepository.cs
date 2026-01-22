using ECommerceService.Api.Data;
using ECommerceService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceService.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public IQueryable<User?> Query()
        {
            return _context.Users.AsNoTracking();
        }
    }
}
