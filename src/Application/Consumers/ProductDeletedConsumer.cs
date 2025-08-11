using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.Extensions.Logging;


namespace Application.Consumers
{
    public class ProductDeletedConsumer: IConsumer<ProductDeletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductDeletedConsumer> _logger;

        public ProductDeletedConsumer(IUnitOfWork unitOfWork, ILogger<ProductDeletedConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            var evt = context.Message;

            _logger.LogInformation(
               "Recebido ProductDeletedEvent: CorrelationId={CorrelationId}, ProductId={ProductId}",
               evt.CorrelationID,
               evt.ProductID);

            var product = await _unitOfWork.Products.GetByIdAsync(evt.ProductID);

            if (product is null)
            {
                _logger.LogWarning(
                    "product com ID {ProductId} não encontrado; nada a remover.",
                    evt.ProductID);
                return;
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation(
                "product com ID {ProductId} removido com sucesso do banco.",
                evt.ProductID);
        }
    }
}
