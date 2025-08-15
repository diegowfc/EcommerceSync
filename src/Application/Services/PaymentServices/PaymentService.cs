using Application.DTOs.PaymentDtos;
using Application.Services.UserServices;
using AutoMapper;
using Domain.Entities.PaymentEntity;
using Domain.Enums.OrderStatus;
using Domain.Event;
using Domain.Interfaces.EndpointCache;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Application.Services.PaymentServices
{
    public class PaymentService(
        IUnitOfWork unitOfWork,
        IEndpointCache endpointCache) : IPaymentService
    {
        private static readonly string QueuePaymentProcess = "payment-processs-commands";
        private readonly Task<ISendEndpoint> _paymentProcessEndpoint = endpointCache.ForQueue(QueuePaymentProcess);
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Guid> ProcessPaymentAsync(PaymentProcessDto paymentDto)
        {
            var correlationId = NewId.NextGuid();

            var paymentProcess = new PaymentProcessComand
            {
                CorrelationId = correlationId,
                OrderId = paymentDto.OrderId,
                PaymentMethod = paymentDto.PaymentMethod,
                CardNumber = paymentDto.CardNumber,
                CardHolder = paymentDto.CardHolder,
                Cvv = paymentDto.Cvv,
                Expiry = paymentDto.Expiry
            };

            var endpoint = await _paymentProcessEndpoint.ConfigureAwait(false);
            await endpoint.Send(paymentProcess).ConfigureAwait(false);

            return correlationId;
        }
    }
}