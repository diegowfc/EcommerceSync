using Domain.Entities.Product;
using Domain.Interfaces.BaseInterface;

namespace Domain.Interfaces.ProductInterface
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> GetProductsInStockAsync();
    }
}
