using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities.ProductEntity;
using Domain.Interfaces.UnitOfWork;

namespace Application.Services.ProductServices
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task CreateProductAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            
            if (product != null) {
                throw new Exception("Produto não encontrado no estoque!");
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductDTO> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(ProductDTO productDto)
        {
            throw new NotImplementedException();
        }
    }
}
