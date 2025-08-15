using MassTransit;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Domain.Interfaces.EndpointCache
{
    public sealed class EndpointCache(IBus bus, ILogger<EndpointCache>? logger = null) : IEndpointCache
    {
        private readonly IBus _bus = bus; 
        private readonly ILogger<EndpointCache>? _logger = logger;

        private readonly ConcurrentDictionary<string, Lazy<Task<ISendEndpoint>>> _cache = new();

        public Task<ISendEndpoint> ForQueue(string queueName)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Queue name is required", nameof(queueName));

            return ForUri(new Uri($"queue:{queueName}"));
        }

        public Task<ISendEndpoint> ForExchange(string exchangeName)
        {
            if (string.IsNullOrWhiteSpace(exchangeName))
                throw new ArgumentException("Exchange name is required", nameof(exchangeName));

            return ForUri(new Uri($"exchange:{exchangeName}"));
        }

        public Task<ISendEndpoint> ForUri(Uri address)
        {
            var key = address.AbsoluteUri;

            var lazy = _cache.GetOrAdd(key, _ =>
                new Lazy<Task<ISendEndpoint>>(
                    () => CreateEndpoint(address),
                    LazyThreadSafetyMode.ExecutionAndPublication));

            return lazy.Value;
        }

        private async Task<ISendEndpoint> CreateEndpoint(Uri address)
        {
            _logger?.LogDebug("Resolving SendEndpoint for {Address}", address);
            var ep = await _bus.GetSendEndpoint(address).ConfigureAwait(false);
            _logger?.LogDebug("Resolved SendEndpoint for {Address}", address);
            return ep;
        }
    }
}
