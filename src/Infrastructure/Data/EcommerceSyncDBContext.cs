using Domain.Entities.CartEntity;
using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Entities.PaymentEntity;
using Domain.Entities.ProductEntity;
using Domain.Entities.UserEntity;
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
        public DbSet<User> tab_user { get; set; }
        public DbSet<Cart> tab_cart { get; set; }
        public DbSet<Payment> tab_payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.Entity<Order>()
                 .Property(o => o.Status)
                 .HasConversion<string>();


            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.Email)
                    .HasColumnType("citext")
                    .IsRequired();

                b.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}