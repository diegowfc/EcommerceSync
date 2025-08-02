using Domain.Entities.CartEntity;
using Domain.Entities.OrderEntity;
using Domain.Interfaces.CartInterface;
using Domain.Interfaces.OrderInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.RepositoryBase;

namespace Infrastructure.Repositories.CartRepository
{
    public class CartRepository(EcommerceSyncDbContext context) : RepositoryBase<Cart>(context), ICartRepository
    {
    }
}
