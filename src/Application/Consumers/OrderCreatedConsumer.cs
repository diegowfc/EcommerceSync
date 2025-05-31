using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Entities.ProductEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.Logging;


namespace Application.Consumers
{
    public class OrderCreatedConsumer: IConsumer<OrderCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IUnitOfWork unitOfWork, ILogger<OrderCreatedConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation("Recebido OrderCreatedEvent: CorrelationId={CorrelationId}", evt.CorrelationId);

            var order = new Order
            {
                DateOfOrder = evt.DateOfOrder,
                OrderIdentifier = evt.OrderIdentifier,
                UserId = evt.UserId,
                Status = evt.Status
            };

            float total = 0f;

            foreach (var itemEvt in evt.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemEvt.ProductId);
                if (product == null)
                {
                    _logger.LogError(
                        "Produto com ID {ProductId} não encontrado para Order {CorrelationId}.",
                        itemEvt.ProductId,
                        evt.CorrelationId);
                    throw new Exception($"Produto com ID {itemEvt.ProductId} não encontrado.");
                }

                product.Stock -= itemEvt.Quantity;
                _unitOfWork.Products.Update(product);
                await _unitOfWork.CommitAsync();

                total += product.Price * itemEvt.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = itemEvt.ProductId,
                    Quantity = itemEvt.Quantity,
                    Product = product
                };

                order.Items.Add(orderItem);
            }

            order.Total = total;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation(
                 "Pedido persistido. OrderId={OrderId}, CorrelationId={CorrelationId}",
                  order.Id,
                  evt.CorrelationId);
        }
    }
}
