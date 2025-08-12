using Domain.Enums.OrderStatus;

namespace Domain.Event
{
    public record OrderRegistrationCommand
    {
        public Guid CorrelationId { get; init; }

        public DateTime DateOfOrder { get; init; }

        public OrderStatus Status { get; init; }

        public float Total { get; init; }

        public string OrderIdentifier { get; init; } = string.Empty;

        public int UserId { get; init; }

        public List<OrderItemCreatedEvent> Items { get; init; } = new();
    }
}
