using Domain.Entities.CartEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Application.Consumers
{
    public class CartAddConsumer(IUnitOfWork unitOfWork) : IConsumer<CartAddCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<CartAddCommand> context)
        {
            var evt = context.Message;

            var cart = new Cart
            {
                ProductId = evt.ProductId,
                UserId = evt.UserId,
                Quantity = evt.Quantity
            };

            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.CommitAsync();
        }
    }
}
