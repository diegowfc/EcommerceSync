using Domain.Entities.ProductEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Consumers
{
    public class OrderDeletedConsumer : IConsumer<OrderDeletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderDeletedConsumer> _logger;

        public OrderDeletedConsumer(IUnitOfWork unitOfWork, ILogger<OrderDeletedConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderDeletedEvent> context)
        {
            var evt = context.Message;

            _logger.LogInformation(
              "Recebido OrderDeletedEvent: CorrelationId={CorrelationId}, OrderId={OrderId}",
              evt.CorrelationID,
              evt.OrderId);

            var order = await _unitOfWork.Orders.GetByIdAsync(evt.OrderId);

            if (order is null)
            {
                _logger.LogWarning(
                    "Pedido com ID {OrderId} não encontrado; nada a remover.",
                    evt.OrderId);
                return;
            }

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation(
                "Pedido com ID {OrderId} removido com sucesso do banco.",
                evt.OrderId);

        }
    }
}
