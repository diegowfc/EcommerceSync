using Domain.Enums.OrderStatus;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Order
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
        public int UserId { get; set; }

        public void GenerateRandomUserId()
        {
            var random = new Random();
            UserId = random.Next(1, 2);
        }

    }
}
