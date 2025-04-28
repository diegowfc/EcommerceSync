using Domain.Entities.Order;
using Domain.Interfaces.BaseInterface;

namespace Domain.Interfaces.OrderInterface
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    }
}