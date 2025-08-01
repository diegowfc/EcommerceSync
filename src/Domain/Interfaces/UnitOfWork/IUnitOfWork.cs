using Domain.Interfaces.OrderInterface;
using Domain.Interfaces.ProductInterface;
using Domain.Interfaces.UserInterface;

namespace Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
