using Domain.Entities.OrderItemEntity;
using Domain.Enums.OrderStatus;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.OrderDtos
{
    public class OrderReadDTO
    {
        public DateTime DateOfOrder { get; set; }
        public string Status { get; set; }
        public float Total { get; set; }
        public string OrderIdentifier { get; set; }
        public int UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
