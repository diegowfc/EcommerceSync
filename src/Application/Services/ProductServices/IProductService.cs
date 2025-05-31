using Application.DTOs.PagedResultsDTO;
using Application.DTOs.ProductDtos;

namespace Application.Services.ProductServices
{
    public interface IProductService
    {
        Task<PagedProductsDTO<ProductDTO>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<ProductCreatedResponseDTO> CreateProductAsync(ProductDTO productDto);
        Task UpdateProductAsync(int id, ProductUpdateDTO productUpdateDto);
        Task DeleteProductAsync(int id);
    }
}
