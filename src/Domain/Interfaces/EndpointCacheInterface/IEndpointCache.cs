using MassTransit;

namespace Domain.Interfaces.EndpointCache
{
    public interface IEndpointCache
    {
        Task<ISendEndpoint> ForQueue(string queueName);
        Task<ISendEndpoint> ForExchange(string exchangeName);
        Task<ISendEndpoint> ForUri(Uri address);
    }
}
