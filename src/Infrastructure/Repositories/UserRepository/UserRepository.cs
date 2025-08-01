using Domain.Entities.UserEntity;
using Domain.Interfaces.UserInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.RepositoryBase;

namespace Infrastructure.Repositories.UserRepository
{
    public class UserRepository: RepositoryBase<User>, IUserRepository
    {
        public UserRepository(EcommerceSyncDbContext context) : base(context)
        {

        }
    }
}
