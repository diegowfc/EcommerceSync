using Application.DTOs.CartDtos;
using AutoMapper;
using Domain.Entities.CartEntity;
using Domain.Event;
using Domain.Interfaces.EndpointCache;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Application.Services.CartServices
{
    public class CartService(IEndpointCache endpointCache) : ICartService
    {
        private static readonly string QueueCartAdd = "cart-add-commands";
        private readonly Task<ISendEndpoint> _cartAddEndpoint = endpointCache.ForExchange(QueueCartAdd);

        public async Task<Guid> AddItemToCartAsync(CartAddDto cartDto)
        {
            var correlationId = NewId.NextGuid();

            var cartAdd = new CartAddCommand
            {
                CorrelationId = correlationId,
                ProductId = cartDto.ProductId,
                UserId = cartDto.UserId,
                Quantity = cartDto.Quantity
            };

            var endpoint = await _cartAddEndpoint.ConfigureAwait(false);
            await endpoint.Send(cartAdd).ConfigureAwait(false);

            return correlationId;
        }
    }
}
