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

    }
}
