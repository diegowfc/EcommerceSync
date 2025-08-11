using Domain.Entities.UserEntity;
using Domain.Interfaces.BaseInterface;

namespace Domain.Interfaces.UserInterface
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByIdAsync(int id);
    }
}