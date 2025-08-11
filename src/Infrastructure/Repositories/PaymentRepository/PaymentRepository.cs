using Domain.Entities.PaymentEntity;
using Domain.Interfaces.PaymentInterface;
using EcommerceSync.Infrastructure.Data;
using Infrastructure.Repositories.RepositoryBase;

namespace Infrastructure.Repositories.PaymentRepository
{
    public class PaymentRepository(EcommerceSyncDbContext context) : RepositoryBase<Payment>(context), IPaymentRepository
    {

    }
}