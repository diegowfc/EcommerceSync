using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Entities.ProductEntity;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}