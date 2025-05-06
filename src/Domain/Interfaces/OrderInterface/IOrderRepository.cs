using Domain.Entities.OrderEntity;
using Domain.Enums.OrderStatus;
using Domain.Interfaces.BaseInterface;

namespace Domain.Interfaces.OrderInterface
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetOrderByIdentifier(string orderId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    }
}