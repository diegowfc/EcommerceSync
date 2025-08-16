using Application.DTOs.PagedResultsDTO;
using Application.DTOs.ProductDtos;
using AutoMapper;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services.ProductServices
{
    public class ProductService(
        ISendEndpointProvider sendEndpointProvider,
        IPublishEndpoint publishEndpoint,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMemoryCache cache) : IProductService
    {
        private static readonly Uri ProductRegistrationUri = new("queue:product-registration-commands");
        private readonly Task<ISendEndpoint> _productRegistrationEndpoint = sendEndpointProvider.GetSendEndpoint(ProductRegistrationUri);
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMemoryCache _cache = cache;


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

        public async Task<PagedProductsDTO<ProductDTO>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var dataQuery = _unitOfWork.Products
                .Query()
                .OrderBy(p => p.Id)
                .Select(p => new ProductDTO { Name = p.Name, Price = p.Price, Stock = p.Stock });

            var total = await _cache.GetOrCreateAsync("products:total", async e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return await _unitOfWork.Products
                    .Query()
                    .CountAsync(cancellationToken);
            });

            var cacheKey = $"products:page:{page}:{pageSize}";

            var items = await _cache.GetOrCreateAsync(cacheKey, async e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return await dataQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            });

            return new PagedProductsDTO<ProductDTO>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task UpdateProductAsync(int id, ProductUpdateDTO productUpdateDto)
        {
            // Em vez de atualizar aqui, publique um ProductUpdatedEvent  
            throw new NotImplementedException();
        }
    }
}
