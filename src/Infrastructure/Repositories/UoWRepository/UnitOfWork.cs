using Domain.Interfaces.OrderInterface;
using Domain.Interfaces.ProductInterface;
using Domain.Interfaces.UnitOfWork;
using Domain.Interfaces.UserInterface;
using EcommerceSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UoWRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EcommerceSyncDbContext _context;

        public IOrderRepository Orders { get; }
        public IProductRepository Products { get; }
        public IUserRepository Users { get; }


        public UnitOfWork(
            EcommerceSyncDbContext context, 
            IOrderRepository orderRepository, 
            IProductRepository productRepository, 
            IUserRepository userRepository)
        {
            _context = context;
            Orders = orderRepository;
            Products = productRepository;
            Users = userRepository;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
