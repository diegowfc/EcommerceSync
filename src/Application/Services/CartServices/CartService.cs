using Application.DTOs.CartDtos;
using AutoMapper;
using Domain.Entities.CartEntity;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Application.Services.CartServices
{
    public class CartService(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ISendEndpointProvider sendEndpointProvider) : ICartService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ISendEndpointProvider _send = sendEndpointProvider;

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

            var endpoint = await _send.GetSendEndpoint(new Uri("queue:cart-add-commands"));
            await endpoint.Send(cartAdd);

            return correlationId;
        }
    }
}
