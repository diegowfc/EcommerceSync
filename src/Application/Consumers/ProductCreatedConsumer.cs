using Domain.Entities.ProductEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.Logging;



namespace Application.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(IUnitOfWork unitOfWork, ILogger<ProductCreatedConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation("Recebido ProductCreatedEvent: CorrelationId={CorrelationId}", evt.CorrelationId);

            var product = new Product
            {
                Name = evt.Name,
                Price = evt.Price,
                Stock = evt.Stock
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Produto persistido no banco. Novo Id={ProductId}", product.Id);

        }
    }
}
