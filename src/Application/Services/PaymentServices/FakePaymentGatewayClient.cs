using Application.DTOs.PaymentDtos;
using Application.Services.PaymentServices;
using System.Collections.Concurrent;

public class FakePaymentGatewayClient : IPaymentGatewayClient
{
    public bool IsAvailable { get; set; } = true;

    private readonly ConcurrentDictionary<string, (GatewayResultDto result, DateTime ts)> _cache
        = new(StringComparer.Ordinal);

    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(30);

    public async Task<GatewayResultDto> ProcessPaymentAsync(
        float amount,
        PaymentProcessDto dto,
        string idempotencyKey,
        CancellationToken ct = default)
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Gateway fora do ar.");

        if (_cache.TryGetValue(idempotencyKey, out var entry) && DateTime.UtcNow - entry.ts < _ttl)
            return entry.result;

        await Task.Delay(500, ct);

        var res = new GatewayResultDto
        {
            Success = true,
            TransactionId = Guid.NewGuid().ToString()
        };
        _cache[idempotencyKey] = (res, DateTime.UtcNow);
        return res;
    }

    public async Task<GatewayResultDto> ProcessPaymentByTokenAsync(
        decimal amount,
        string paymentToken,
        string idempotencyKey,
        CancellationToken ct = default)
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Gateway fora do ar.");

        if (_cache.TryGetValue(idempotencyKey, out var entry) && DateTime.UtcNow - entry.ts < _ttl)
            return entry.result;

        await Task.Delay(500, ct);

        var res = new GatewayResultDto
        {
            Success = true,
            TransactionId = Guid.NewGuid().ToString()
        };
        _cache[idempotencyKey] = (res, DateTime.UtcNow);
        return res;
    }
}
