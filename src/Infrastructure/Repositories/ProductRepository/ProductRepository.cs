using Domain.Entities.ProductEntity;
using Domain.Interfaces.ProductInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(EcommerceSyncDbContext context) : base(context)
        {
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsInStockAsync()
        {
            return await _dbSet.Where(p => p.Stock > 0).ToListAsync();
        }

        public IQueryable<Product> Query()
        {
            return _dbSet;
        }
    }
}
