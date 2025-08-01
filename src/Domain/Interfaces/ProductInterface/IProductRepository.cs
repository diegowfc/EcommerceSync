using Domain.Entities.ProductEntity;
using Domain.Interfaces.BaseInterface;

namespace Domain.Interfaces.ProductInterface
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Product> GetProductByIdAsync(int id);
        IQueryable<Product> Query();
    }
}
