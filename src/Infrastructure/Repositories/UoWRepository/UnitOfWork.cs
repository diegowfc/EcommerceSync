using Domain.Interfaces.CartInterface;
using Domain.Interfaces.OrderInterface;
using Domain.Interfaces.PaymentInterface;
using Domain.Interfaces.ProductInterface;
using Domain.Interfaces.UnitOfWork;
using Domain.Interfaces.UserInterface;
using EcommerceSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UoWRepository
{
    public class UnitOfWork(
        EcommerceSyncDbContext context,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserRepository userRepository,
        ICartRepository cartRepository,
        IPaymentRepository paymentRepository) : IUnitOfWork
    {
        private readonly EcommerceSyncDbContext _context = context;

        public IOrderRepository Orders { get; } = orderRepository;
        public IProductRepository Products { get; } = productRepository;
        public IUserRepository Users { get; } = userRepository;
        public ICartRepository Carts { get; } = cartRepository;
        public IPaymentRepository Payments { get; } = paymentRepository;

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
