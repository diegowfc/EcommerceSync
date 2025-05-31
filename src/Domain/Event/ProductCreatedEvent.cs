using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    public record ProductCreatedEvent
    {
        public Guid CorrelationId { get; init; }     
        public string Name { get; init; }
        public float Price { get; init; }
        public int Stock { get; init; }
    }
}
