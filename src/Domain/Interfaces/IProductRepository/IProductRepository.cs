using Domain.Entities.Product;
using Domain.Interfaces.IRepositoryBase;

namespace Domain.Interfaces.IProductRepository
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> GetProductsInStockAsync();
    }
}
