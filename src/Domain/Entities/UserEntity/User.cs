using Domain.Entities.CartEntity;
using Domain.Entities.OrderEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.UserEntity
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [NotMapped]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        [Required]
        public string Email { get; set; }

        public ICollection<Cart> Carts { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];

    }
}