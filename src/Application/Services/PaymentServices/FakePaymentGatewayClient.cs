using Application.DTOs.PaymentDtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PaymentServices
{
    public class FakePaymentGatewayClient : IFakePaymentGatewayClient
    {
        public bool IsAvailable { get; set; } = true;
        private readonly ConcurrentDictionary<string, (GatewayResultDto res, DateTime ts)> _cache = new();
        private readonly TimeSpan _ttl = TimeSpan.FromMinutes(30);

        public async Task<GatewayResultDto> ProcessPaymentByTokenAsync(float amount, string paymentToken, string idempotencyKey, CancellationToken ct = default)
        {
            if (!IsAvailable) throw new InvalidOperationException("Gateway fora do ar.");

            if (_cache.TryGetValue(idempotencyKey, out var e) && DateTime.UtcNow - e.ts < _ttl)
                return e.res;

            await Task.Delay(500, ct);

            var res = new GatewayResultDto { Success = true, TransactionId = "txn_" + Guid.NewGuid().ToString("N")[..24] };
            _cache[idempotencyKey] = (res, DateTime.UtcNow);
            return res;
        }


        public async Task<string> TokenizeCardAsync(string cardNumber, string cardHolder, string expiry, string cvv, CancellationToken ct = default)
        {
            if (!IsAvailable) throw new InvalidOperationException("Gateway fora do ar.");
            await Task.Delay(100, ct);
            return "tok_" + Guid.NewGuid().ToString("N")[..24];
        }
    }
}