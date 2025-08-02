using Application.DTOs.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PaymentServices
{
    public class FakePaymentGatewayClient : IPaymentGatewayClient
    {
        public Task<GatewayResultDto> ProcessPaymentAsync(float amount, PaymentProcessDto dto)
        {
            Thread.Sleep(500);

            var success = new Random().NextDouble() > 0.1;
            return Task.FromResult(new GatewayResultDto
            {
                Success = success,
                TransactionId = success ? Guid.NewGuid().ToString() : null,
                ErrorMessage = success ? null : "Cartão recusado"
            });
        }
    }
}
