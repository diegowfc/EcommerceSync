using Application.DTOs.PaymentDtos;
using Application.Services.UserServices;
using AutoMapper;
using Domain.Entities.PaymentEntity;
using Domain.Enums.OrderStatus;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Application.Services.PaymentServices
{
    public class PaymentService(
        IUnitOfWork unitOfWork, 
        ISendEndpointProvider sendEndpointProvider): IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISendEndpointProvider _send = sendEndpointProvider;

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

            var endpoint = await _send.GetSendEndpoint(new Uri("queue:payment-processs-commands"));
            await endpoint.Send(paymentProcess);

            return correlationId;

        }
    }
}