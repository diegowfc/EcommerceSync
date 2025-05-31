namespace Application.DTOs.ProductDtos
{
    public class ProductCreatedResponseDTO
    {
        public Guid CorrelationId { get; init; }
        public string Message { get; init; }
    }
}
