using Domain.Enums.OrderStatus;

namespace Application.DTOs
{
    public class OrderDTO
    {
        public float Total { get; set; }
        public OrderStatus Status { get; set; }
        public int UserId { get; set; }

        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
