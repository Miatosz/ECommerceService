using ECommerceService.Api.Models;

namespace ECommerceService.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        IQueryable<User?> Query();
        Task AddAsync(User user);
    }
}
