using Domain.Entities.OrderItemEntity;
using Domain.Enums.OrderStatus;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.OrderEntity
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateOfOrder { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public float Total {  get; set; }
        [Required]
        [MaxLength(30)]
        public string OrderIdentifier { get; set; } = string.Empty;
        [Required]
        public int UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();

        public void GenerateRandomUserId()
        {
            var random = new Random();
            UserId = random.Next(1, 2);
        }

    }
}
