using Application.Services.PaymentServices;
using Domain.Entities.CartEntity;
using Domain.Entities.OrderEntity;
using Domain.Entities.PaymentEntity;
using Domain.Enums.OrderStatus;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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

            var order = await _unitOfWork.Orders.Query()
                .AsNoTracking()
                .Where(x => x.Id == evt.OrderId)
                .Select(x => new { x.Id, x.Total, x.Status })
                .FirstOrDefaultAsync(context.CancellationToken);

            if (order is null) return;

            var result = await gateway.ProcessPaymentAsync(context.CancellationToken);

            var newStatus = result.Success ? OrderStatus.Paid : OrderStatus.Failed;

            var orderUpdate = new Order { Id = order.Id, Status = newStatus };

            unitOfWork.Orders.Attach(orderUpdate);
            unitOfWork.Orders.MarkStatusModified(orderUpdate);

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
