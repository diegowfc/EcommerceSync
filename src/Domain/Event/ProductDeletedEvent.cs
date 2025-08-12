using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    public class ProductDeletedEvent
    {
        public Guid CorrelationID { get; init; }
        public int ProductID { get; init; }
    }
}
