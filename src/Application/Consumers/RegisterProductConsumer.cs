using Domain.Entities.ProductEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Consumers
{
    public class RegisterProductConsumer(IUnitOfWork unitOfWork) : IConsumer<ProductRegistrationCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<ProductRegistrationCommand> context)
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
