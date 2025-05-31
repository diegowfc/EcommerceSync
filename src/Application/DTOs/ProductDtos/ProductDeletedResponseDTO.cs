using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDtos
{
    public class ProductDeletedResponseDTO
    {
        public Guid CorrelationId { get; init; }
        public string Message { get; init; }
    }
}
