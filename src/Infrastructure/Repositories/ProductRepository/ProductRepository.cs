using Domain.Entities.Product;
using Domain.Interfaces.IProductRepository;
using Infrastructure.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsInStockAsync()
        {
            return await _dbSet.Where(p => p.Stock > 0).ToListAsync();
        }
    }
}
