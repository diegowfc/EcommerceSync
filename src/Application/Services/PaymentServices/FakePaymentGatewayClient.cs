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

        public Task<GatewayResultDto> ProcessPaymentAsync()
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Gateway fora do ar.");

            Thread.Sleep(500);

            return Task.FromResult(new GatewayResultDto
            {
                Success = true,
                TransactionId = Guid.NewGuid().ToString()
            });
        }
    }
}