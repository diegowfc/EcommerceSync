using Domain.Entities.PaymentEntity;
using Domain.Interfaces.BaseInterface;

namespace Domain.Interfaces.PaymentInterface
{
    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
    }
}