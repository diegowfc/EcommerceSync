// Infrastructure/Consumers/OrderCreatedConsumer.cs
using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Infrastructure.Consumers
{
    public class RegisterOrderConsumer(IUnitOfWork unitOfWork) : IConsumer<OrderRegistrationCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<OrderRegistrationCommand> context)
        {
            var evt = context.Message;

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
                    throw new Exception($"product com ID {itemEvt.ProductId} não encontrado.");
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
        }
    }
}
