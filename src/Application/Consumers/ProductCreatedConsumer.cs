using Domain.Entities.ProductEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Consumers
{
    public class ProductCreatedConsumer(IUnitOfWork unitOfWork) : IConsumer<ProductCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            var evt = context.Message;

            var product = new Product
            {
                Name = evt.Name,
                Price = evt.Price,
                Stock = evt.Stock
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();
        }
    }
}
