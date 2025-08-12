using Application.Services.PaymentServices;
using Domain.Entities.CartEntity;
using Domain.Entities.PaymentEntity;
using Domain.Enums.OrderStatus;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Consumers
{
    public class PaymentProcessConsumer(IUnitOfWork unitOfWork, IFakePaymentGatewayClient gateway) : IConsumer<PaymentProcessComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFakePaymentGatewayClient _gateway = gateway;

        public async Task Consume(ConsumeContext<PaymentProcessComand> context)
        {
            var evt = context.Message;

            var order = await _unitOfWork.Orders.GetByIdAsync(evt.OrderId);

            if (order is null) return;

            var result = await gateway.ProcessPaymentAsync();

            order.Status = result.Success ? OrderStatus.Paid : OrderStatus.Failed;
            _unitOfWork.Orders.Update(order);

            var payment = new Payment
            {
                Amount = order.Total, 
                TransactionId = result.TransactionId,
                Success = result.Success,
                CreatedAt = DateTime.UtcNow,
                OrderId = order.Id
            };

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CommitAsync();
        }
    }
}
