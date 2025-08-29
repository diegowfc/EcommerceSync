using Domain.Event;
using Domain.Entities.PaymentEntity;
using Domain.Enums.OrderStatus;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Application.Services.PaymentServices;

public class PaymentProcessConsumer(IUnitOfWork uow, IFakePaymentGatewayClient gateway) : IConsumer<PaymentProcessComand>
{
    public async Task Consume(ConsumeContext<PaymentProcessComand> ctx)
    {
        var msg = ctx.Message;

        var order = await uow.Orders.GetByIdAsync(msg.OrderId)
                   ?? throw new KeyNotFoundException($"Order {msg.OrderId} não encontrada.");

        if (order.Status == OrderStatus.Paid)
            return; 

        var amount = order.Total;
        var idempotencyKey = $"order:{order.Id}";

        var result = await gateway.ProcessPaymentByTokenAsync(amount, msg.PaymentToken, idempotencyKey);

        var payment = new Payment
        {
            OrderId = order.Id,
            Amount = amount,
            TransactionId = result.TransactionId,
            Success = result.Success,
            CreatedAt = DateTime.UtcNow
        };
        await uow.Payments.AddAsync(payment);

        order.Status = result.Success ? OrderStatus.Paid : OrderStatus.Failed;
        uow.Orders.Update(order);

        await uow.CommitAsync();
    }
}
