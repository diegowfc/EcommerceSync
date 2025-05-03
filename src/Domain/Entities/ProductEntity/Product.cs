using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ProductEntity
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Column(TypeName ="decimal(18,2")]
        public float Price { get; set; }
        [Required] 
        public int Stock {  get; set; }
    }
}
