using Domain.Enums.OrderStatus;

namespace Application.DTOs
{
    public class OrderDTO
    {
        public DateTime DateOfOrder { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public int UserId { get; set; }

        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
