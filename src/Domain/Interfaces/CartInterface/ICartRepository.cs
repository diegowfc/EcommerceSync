using Domain.Entities.CartEntity;
using Domain.Interfaces.BaseInterface;

namespace Domain.Interfaces.CartInterface
{
    public interface ICartRepository: IRepositoryBase<Cart>
    {
        IQueryable<Cart> Query();
    }
}
