using Domain.Entities.OrderEntity;
using Domain.Enums.OrderStatus;
using Domain.Interfaces.OrderInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.OrderRepository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(EcommerceSyncDbContext context) : base(context)
        {

        }
    }
}
