using Application.DTOs.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PaymentServices
{
    public interface IFakePaymentGatewayClient
    {

        Task<GatewayResultDto> ProcessPaymentByTokenAsync(float amount, string paymentToken, string idempotencyKey, CancellationToken ct = default);
        Task<string> TokenizeCardAsync(string cardNumber, string cardHolder, string expiry, string cvv, CancellationToken ct = default);
    }
}