using Application.DTOs.PagedResultsDTO;
using Application.DTOs.ProductDtos;
using AutoMapper;
using Domain.Entities.ProductEntity;
using Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace Application.Services.ProductServices
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<int> CreateProductAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return product.Id;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            
            if (product == null) {
                throw new Exception("Produto não encontrado no estoque!");
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CommitAsync();
        }

        public async Task<CursorPage<ProductDTO>> GetProductsAsync(int? afterId, int pageSize, CancellationToken cancellationToken = default)
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
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Produto não encontrado no estoque!");

            if (productUpdateDto.Name != null)
                product.Name = productUpdateDto.Name;

            if (productUpdateDto.Price.HasValue)
                product.Price = productUpdateDto.Price.Value;

            if (productUpdateDto.Stock.HasValue)
                product.Stock = productUpdateDto.Stock.Value;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.CommitAsync();

        }
    }
}
