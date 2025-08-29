using Domain.Entities.CartEntity;
using Domain.Interfaces.CartInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.RepositoryBase;

namespace Infrastructure.Repositories.CartRepository
{
    public class CartRepository(EcommerceSyncDbContext context) : RepositoryBase<Cart>(context), ICartRepository
    {
        public IQueryable<Cart> Query()
        {
            return _dbSet;
        }
    }
}
