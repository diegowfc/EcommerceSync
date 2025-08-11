using Application.DTOs.CartDtos;
using AutoMapper;
using Domain.Entities.CartEntity;
using Domain.Interfaces.UnitOfWork;

namespace Application.Services.CartServices
{
    public class CartService(IUnitOfWork unitOfWork, IMapper mapper) : ICartService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task AddItemToCartAsync(CartAddDto cartDto)
        {
            var userCart = _mapper.Map<Cart>(cartDto);
            await _unitOfWork.Carts.AddAsync(userCart);
            await _unitOfWork.CommitAsync();
        }
    }
}
