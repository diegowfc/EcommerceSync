using Domain.Interfaces.OrderInterface;
using Domain.Interfaces.ProductInterface;

namespace Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        Task<int> CommitAsync();
    }
}
