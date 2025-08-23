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

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            var e = email?.Trim();
            return await _dbSet.AsNoTracking().AnyAsync(u => u.Email == e, ct);
        }
    }
}
