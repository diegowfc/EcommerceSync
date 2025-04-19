using Microsoft.EntityFrameworkCore;

namespace EcommerceSync.Infrastructure.Data
{
    public class EcommerceSyncDbContext : DbContext
    {
        public EcommerceSyncDbContext(DbContextOptions<EcommerceSyncDbContext> options)
            : base(options)
        { }
    }
}