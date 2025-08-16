using Application.DTOs.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PaymentServices
{
    public class FakePaymentGatewayClient : IFakePaymentGatewayClient
    {
        public bool IsAvailable { get; set; } = true;

        public async Task<GatewayResultDto> ProcessPaymentAsync(CancellationToken ct = default)
        {
            if (!IsAvailable) throw new HttpRequestException("Gateway offline");
            await Task.Delay(500, ct);
            return new GatewayResultDto { Success = true, TransactionId = Guid.NewGuid().ToString() };
        }
    }
}