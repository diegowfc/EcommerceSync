using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.CartEntity
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
        public int Quantity { get; set; }
    }
}
