using Domain.Entities.Order;
using Domain.Entities.OrderItem;
using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace EcommerceSync.Infrastructure.Data
{
    public class EcommerceSyncDbContext : DbContext
    {
        public EcommerceSyncDbContext(DbContextOptions<EcommerceSyncDbContext> options)
            : base(options)
        { }

        public DbSet<Product> tab_products { get; set; }
        public DbSet<Order> tab_order { get; set; }
        public DbSet<OrderItem> tab_order_item { get; set; }

    }
}