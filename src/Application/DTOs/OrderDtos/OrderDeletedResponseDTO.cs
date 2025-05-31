using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OrderDtos
{
    public class OrderDeletedResponseDTO
    {
        public Guid CorrelationId { get; init; }
        public string Message { get; init; }
    }
}
