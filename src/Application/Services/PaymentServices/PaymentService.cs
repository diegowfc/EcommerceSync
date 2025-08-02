using Application.DTOs.PaymentDtos;
using Application.Services.UserServices;
using AutoMapper;
using Domain.Entities.PaymentEntity;
using Domain.Enums.OrderStatus;
using Domain.Interfaces.UnitOfWork;

namespace Application.Services.PaymentServices
{
    public class PaymentService(IUnitOfWork unitOfWork, IPaymentGatewayClient _gateway) : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPaymentGatewayClient _gateway = _gateway;

        public async Task<GatewayResultDto> ProcessAsync(PaymentProcessDto dto)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(dto.OrderId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order {dto.OrderId} não encontrada.");
            }

            var result = await _gateway.ProcessPaymentAsync(order.Total, dto);

            var payment = new Payment
            {
                OrderId = dto.OrderId,
                Amount = order.Total,
                TransactionId = result.TransactionId,
                Success = result.Success,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Payments.AddAsync(payment);

            order.Status = result.Success ? OrderStatus.Paid : OrderStatus.Failed;
            _unitOfWork.Orders.Update(order);

            await _unitOfWork.CommitAsync();

            return result;

        }
    }
}
