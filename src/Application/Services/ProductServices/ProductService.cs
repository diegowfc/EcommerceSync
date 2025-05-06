using Application.DTOs.ProductDtos;
using AutoMapper;
using Domain.Entities.ProductEntity;
using Domain.Interfaces.UnitOfWork;

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

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var products = await _unitOfWork.Products.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(products);
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
