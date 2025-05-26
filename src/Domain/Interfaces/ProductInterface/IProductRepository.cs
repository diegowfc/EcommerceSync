using Domain.Entities.ProductEntity;
using Domain.Interfaces.BaseInterface;

namespace Domain.Interfaces.ProductInterface
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> GetProductsInStockAsync();
        Task<Product> GetProductByIdAsync(int id);
        IQueryable<Product> Query();
    }
}
