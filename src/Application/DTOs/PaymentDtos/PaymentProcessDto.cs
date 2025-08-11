using Domain.Enums.PaymentMethods;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.PaymentDtos
{
    public class PaymentProcessDto
    {
        public int OrderId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        [CreditCard]
        public string CardNumber { get; set; }

        public string CardHolder { get; set; }

        public string Expiry { get; set; }

        public string Cvv { get; set; }
    }
}