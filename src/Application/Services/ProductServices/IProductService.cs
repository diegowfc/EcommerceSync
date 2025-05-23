﻿using Application.DTOs.ProductDtos;

namespace Application.Services.ProductServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<int> CreateProductAsync(ProductDTO productDto);
        Task UpdateProductAsync(int id, ProductUpdateDTO productUpdateDto);
        Task DeleteProductAsync(int id);
    }
}
