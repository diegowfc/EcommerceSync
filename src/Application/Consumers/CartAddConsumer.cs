using Domain.Entities.CartEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Application.Consumers
{
    public class CartAddConsumer(IUnitOfWork unitOfWork) : IConsumer<CartAddCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<CartAddCommand> context)
        {
            var evt = context.Message;

            var affected = await _unitOfWork.Carts.Query()
                .Where(c => c.UserId == evt.UserId && c.ProductId == evt.ProductId)
                .ExecuteUpdateAsync(set => set
               .SetProperty(c => c.Quantity, c => c.Quantity + evt.Quantity));

            if (affected == 0)
            {
                try
                {
                    await _unitOfWork.Carts.AddAsync(new Domain.Entities.CartEntity.Cart
                    {
                        UserId = evt.UserId,
                        ProductId = evt.ProductId,
                        Quantity = evt.Quantity
                    });
                    await _unitOfWork.CommitAsync();
                    return;
                }
                catch (DbUpdateException)
                {
                    // Corrida: outra instância inseriu; faz UPDATE
                    await _unitOfWork.Carts.Query()
                        .Where(c => c.UserId == evt.UserId && c.ProductId == evt.ProductId)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(c => c.Quantity, c => c.Quantity + evt.Quantity));
                }
            }

            await _unitOfWork.CommitAsync();

        }
    }
}
