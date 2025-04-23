using Domain.Entities.Order;
using Domain.Interfaces.IRepositoryBase;

namespace Domain.Interfaces.IOrderRepository
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    }
}