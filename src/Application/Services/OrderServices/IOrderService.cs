using Application.DTOs;
using Domain.Enums.OrderStatus;

namespace Application.Services.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task CreateOrderAsync(OrderDTO dto);
        Task UpdateOrderAsync(int id,OrderDTO dto);
        Task DeleteOrderAsync(int id);
        Task UpdateOrderStatusAsync(int id, OrderDTO dto);
    }
}
