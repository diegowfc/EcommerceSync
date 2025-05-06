using Application.DTOs.OrderItemDtos;
using Domain.Enums.OrderStatus;

namespace Application.DTOs.OrderDtos
{
    public class OrderDTO
    {
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
