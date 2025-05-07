using Application.DTOs.OrderDtos;

namespace Application.Services.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDTO>> GetAllOrdersAsync();
        Task<OrderReadDTO> GetOrderByIdAsync(int id);
        Task<int> CreateOrderAsync(OrderDTO dto);
        Task UpdateOrderAsync(int id,OrderUpdateDTO dto);
        Task DeleteOrderAsync(int id);
        Task UpdateOrderStatusAsync(int id, OrderUpdateDTO dto);
        string GenerateOrderIdentifier(DateTime orderDate);
    }
}
