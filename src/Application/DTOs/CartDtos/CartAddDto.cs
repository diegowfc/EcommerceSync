namespace Application.DTOs.CartDtos
{
    public class CartAddDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}