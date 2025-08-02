using Domain.Entities.ProductEntity;
using Domain.Interfaces.ProductInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository(EcommerceSyncDbContext context) : RepositoryBase<Product>(context), IProductRepository
    {
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Product> Query()
        {
            return _dbSet;
        }
    }
}
