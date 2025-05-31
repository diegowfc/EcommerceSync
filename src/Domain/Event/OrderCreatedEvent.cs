using Domain.Entities.OrderItemEntity;
using Domain.Enums.OrderStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    public record OrderCreatedEvent
    {
        public Guid CorrelationId { get; set; }

        public DateTime DateOfOrder { get; set; }
        
        public OrderStatus Status { get; set; }

        public float Total { get; set; }

        public string OrderIdentifier { get; set; } = string.Empty;

        public int UserId { get; set; }

        public List<OrderItemCreatedEvent> Items { get; init; } = new();
    }
}
