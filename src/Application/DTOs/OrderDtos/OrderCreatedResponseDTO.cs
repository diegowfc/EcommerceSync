namespace Application.DTOs.OrderDtos
{
    public class OrderCreatedResponseDTO
    {
        public Guid CorrelationId { get; init; }
        public string Message { get; init; }
    }
}
