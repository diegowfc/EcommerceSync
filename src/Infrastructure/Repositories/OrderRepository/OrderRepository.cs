using Domain.Entities.OrderEntity;
using Domain.Entities.ProductEntity;
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

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _dbSet.Where(order => order.Status == status).ToListAsync();
        }

        public IQueryable<Order> Query()
        {
            return _dbSet;
        }

        public new void Attach(Order entity) => base.Attach(entity);

        public void MarkStatusModified(Order entity)
        {
            Attach(entity);
            _context.Entry(entity).Property(o => o.Status).IsModified = true;
        }
    }
}
