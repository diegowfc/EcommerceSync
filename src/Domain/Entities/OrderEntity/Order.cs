using Domain.Entities.OrderItemEntity;
using Domain.Entities.PaymentEntity;
using Domain.Entities.UserEntity;
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

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<OrderItem> Items { get; set; } = [];

        public ICollection<Payment> Payments { get; set; } = [];


    }
}
