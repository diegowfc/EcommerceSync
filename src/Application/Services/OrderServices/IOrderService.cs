using Application.DTOs.OrderDtos;

namespace Application.Services.OrderServices
{
    public interface IOrderService
    {
        Task<OrderReadDTO> GetOrderByIdAsync(int id);
        Task<OrderCreatedResponseDTO> CreateOrderAsync(OrderDTO dto);
        Task UpdateOrderAsync(int id,OrderUpdateDTO dto);
        Task<OrderDeletedResponseDTO> DeleteOrderAsync(int id);
        Task UpdateOrderStatusAsync(int id, OrderUpdateDTO dto);
        string GenerateOrderIdentifier(DateTime orderDate);
    }
}
