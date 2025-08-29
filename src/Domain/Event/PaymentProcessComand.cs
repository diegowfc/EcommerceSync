using Domain.Enums.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    public class PaymentProcessComand
    {
        public Guid CorrelationId { get; init; }
        public int OrderId { get; init; }
        public string PaymentToken { get; set; } = default!;


    }
}
