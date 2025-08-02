using Domain.Entities.UserEntity;
using Domain.Interfaces.UserInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UserRepository
{
    public class UserRepository(EcommerceSyncDbContext context) : RepositoryBase<User>(context), IUserRepository
    {
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
