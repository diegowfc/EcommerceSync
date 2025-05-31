using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    public class OrderDeletedEvent
    {
        public Guid CorrelationID { get; set; }
        public int OrderId { get; set; }
    }
}
