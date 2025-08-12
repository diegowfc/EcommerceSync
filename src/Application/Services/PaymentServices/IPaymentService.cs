using Application.DTOs.PaymentDtos;

namespace Application.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<Guid> ProcessPaymentAsync(PaymentProcessDto dto);
    }
}