using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Entities.ProductEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
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
            _logger.LogInformation(
                "Recebido OrderCreatedEvent: CorrelationId={CorrelationId}",
                evt.CorrelationId);

            var productIds = evt.Items.Select(i => i.ProductId).Distinct().ToList();

            var productsOrdered = await _unitOfWork
                .Products
                .Query()
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            var order = new Order
            {
                DateOfOrder = evt.DateOfOrder,
                OrderIdentifier = evt.OrderIdentifier,
                UserId = evt.UserId,
                Status = evt.Status,
                Items = new List<OrderItem>()
            };

            float total = 0f;

            foreach (var itemEvt in evt.Items)
            {
                var product = productsOrdered.Single(p => p.Id == itemEvt.ProductId);

                product.Stock -= itemEvt.Quantity;

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
