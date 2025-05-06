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

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrderByIdentifier(string orderId)
        {
            return await _dbSet.Where(x => x.OrderIdentifier == orderId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _dbSet.Where(order => order.Status == status).ToListAsync();
        }

    }
}
