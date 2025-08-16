using Application.DTOs.PagedResultsDTO;
using Application.DTOs.ProductDtos;

namespace Application.Services.ProductServices
{
    public interface IProductService
    {
        Task<CursorPage<ProductDTO>> GetProductsAsync(int? afterId, int pageSize, CancellationToken cancellationToken = default);
        Task<int> CreateProductAsync(ProductDTO productDto);
        Task UpdateProductAsync(int id, ProductUpdateDTO productUpdateDto);
        Task DeleteProductAsync(int id);
    }
}
