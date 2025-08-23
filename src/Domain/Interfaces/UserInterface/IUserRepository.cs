using Domain.Entities.ProductEntity;
using Domain.Entities.UserEntity;
using Domain.Interfaces.BaseInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.UserInterface
{
    public interface IUserRepository: IRepositoryBase<User>
    {
        Task<User> GetUserByIdAsync(int id);
        Task<bool> ExistsByEmailAsync(string normalizedEmail, CancellationToken ct = default);
    }
}
