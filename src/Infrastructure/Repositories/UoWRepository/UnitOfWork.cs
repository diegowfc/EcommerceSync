using Domain.Interfaces.CartInterface;
using Domain.Interfaces.OrderInterface;
using Domain.Interfaces.PaymentInterface;
using Domain.Interfaces.ProductInterface;
using Domain.Interfaces.UnitOfWork;
using Domain.Interfaces.UserInterface;
using EcommerceSync.Infrastructure.Data;

namespace Infrastructure.Repositories._unitOfWorkRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EcommerceSyncDbContext _context;

        public IOrderRepository Orders { get; }
        public IProductRepository Products { get; }
        public IPaymentRepository Payments { get; }
        public ICartRepository Carts { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(EcommerceSyncDbContext context, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _context = context;
            Orders = orderRepository;
            Products = productRepository;
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
