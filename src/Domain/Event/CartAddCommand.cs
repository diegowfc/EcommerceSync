using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    public class CartAddCommand
    {
        public Guid CorrelationId { get; init; }
        public int UserId { get; init; }
        public int ProductId { get; init; }
        public int Quantity { get; init; }
    }
}
