using Application.DTOs.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PaymentServices
{
    public interface IPaymentGatewayClient
    {
        Task<GatewayResultDto> ProcessPaymentAsync(float amount, PaymentProcessDto dto, string idempotencyKey, CancellationToken ct);
    }
}
