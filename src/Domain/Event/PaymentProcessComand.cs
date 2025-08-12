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
        public PaymentMethod PaymentMethod { get; init; }
        public string CardNumber { get; init; }
        public string CardHolder { get; init; }
        public string Expiry { get; init; }
        public string Cvv { get; init; }

    }
}
