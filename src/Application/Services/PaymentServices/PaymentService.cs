using Application.DTOs.PaymentDtos;
using Domain.Enums.OrderStatus;
using Domain.Event;
using Domain.Interfaces.EndpointCache;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Application.Services.PaymentServices
{
    public class PaymentService(IUnitOfWork unitOfWork, IEndpointCache endpointCache, IFakePaymentGatewayClient gateway) : IPaymentService
    {
        private static readonly string QueuePaymentProcess = "payment-processs-commands";
        private readonly Task<ISendEndpoint> _endpoint = endpointCache.ForQueue(QueuePaymentProcess);
        private readonly IUnitOfWork _uow = unitOfWork;
        private readonly IFakePaymentGatewayClient _gateway = gateway;

        public async Task<Guid> ProcessPaymentAsync(PaymentProcessDto dto)
        {
            var order = await _uow.Orders.GetByIdAsync(dto.OrderId)
                       ?? throw new KeyNotFoundException($"Order {dto.OrderId} não encontrada.");

            if (order.Status == OrderStatus.Paid)
                return Guid.Empty;

            if (string.IsNullOrWhiteSpace(dto.PaymentToken))
            {
                if (string.IsNullOrWhiteSpace(dto.CardNumber) ||
                    string.IsNullOrWhiteSpace(dto.CardHolder) ||
                    string.IsNullOrWhiteSpace(dto.Expiry) ||
                    string.IsNullOrWhiteSpace(dto.Cvv))
                {
                    throw new ArgumentException("Dados completos do cartão são obrigatórios.");
                }

                dto.PaymentToken = await _gateway.TokenizeCardAsync(dto.CardNumber, dto.CardHolder, dto.Expiry, dto.Cvv);
            }

            var correlationId = NewId.NextGuid();

            var cmd = new PaymentProcessComand
            {
                CorrelationId = correlationId,
                OrderId = dto.OrderId,
                PaymentToken = dto.PaymentToken!
            };

            var ep = await _endpoint.ConfigureAwait(false);
            await ep.Send(cmd).ConfigureAwait(false);

            return correlationId; 
        }
    }
}
