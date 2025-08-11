using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.PaymentDtos
{
    public class GatewayResultDto
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
    }
}