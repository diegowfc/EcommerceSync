using Application.DTOs.PagedResultsDTO;
using Application.DTOs.ProductDtos;
using AutoMapper;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ProductServices
{
    public class ProductService(
        ISendEndpointProvider sendEndpointProvider,
        IPublishEndpoint publishEndpoint,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IProductService
    {
        private static readonly Uri ProductRegistrationUri = new("queue:product-registration-commands");
        private readonly Task<ISendEndpoint> _productRegistrationEndpoint = sendEndpointProvider.GetSendEndpoint(ProductRegistrationUri);
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ISendEndpointProvider _send = sendEndpointProvider;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ProductCreatedResponseDTO> CreateProductAsync(ProductDTO productDto)
        {
            var correlationId = Guid.NewGuid();

            var productRegistration = new ProductRegistrationCommand
            {
                CorrelationId = correlationId,
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            var endpoint = await _productRegistrationEndpoint;
            await endpoint.Send(productRegistration);

            return new ProductCreatedResponseDTO
            {
                CorrelationId = correlationId,
                Message = $"Evento de criação do product iniciado (ID: {correlationId})"
            };
        }

        public async Task<ProductDeletedResponseDTO> DeleteProductAsync(int id)
        {
            var exists = await _unitOfWork.Products.GetByIdAsync(id);

            if (exists is null)
                throw new Exception($"product com ID {id} não encontrado.");

            var correlationId = Guid.NewGuid();

            await _publishEndpoint.Publish(new ProductDeletedEvent
            {
                CorrelationID = correlationId,
                ProductID = id
            });

            var productDeletedResponse = new ProductDeletedResponseDTO
            {
                CorrelationId = correlationId,
                Message = $"Evento de delete do product iniciado (ID: {correlationId})"
            };

            return productDeletedResponse;
        }

        public async Task<CursorPage<ProductDTO>> GetProductsAsync( int? afterId, int pageSize, CancellationToken cancellationToken = default)
        {
            pageSize = Math.Clamp(pageSize, 1, 100);

            var items = await _unitOfWork.Products.Query()
                .AsNoTracking()
                .Where(p => afterId == null || p.Id > afterId)
                .OrderBy(p => p.Id)
                .Take(pageSize + 1)             
                .Select(p => new ProductDTO
                {
                    Id = p.Id,                   
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock
                })
                .ToListAsync(cancellationToken);

            var hasMore = items.Count > pageSize;
            if (hasMore) items.RemoveAt(items.Count - 1);

            var nextAfter = items.Count > 0 ? items[^1].Id : (int?)null;

            return new CursorPage<ProductDTO>
            {
                Items = items,
                NextAfter = nextAfter,
                HasMore = hasMore
            };
        }

        public async Task UpdateProductAsync(int id, ProductUpdateDTO productUpdateDto)
        {
            // Em vez de atualizar aqui, publique um ProductUpdatedEvent  
            throw new NotImplementedException();
        }
    }
}
