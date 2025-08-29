using Application.DTOs.CartDtos;
using AutoMapper;
using Domain.Entities.CartEntity;
using Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CartServices
{
    public class CartService(IUnitOfWork unitOfWork, IMapper mapper) : ICartService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task AddItemToCartAsync(CartAddDto cartDto)
        {
            var affected = await _unitOfWork.Carts.Query()
                .Where(c => c.UserId == cartDto.UserId && c.ProductId == cartDto.ProductId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(c => c.Quantity, c => c.Quantity + cartDto.Quantity));

            if (affected > 0)
            {
                await _unitOfWork.CommitAsync();
                return;
            }

            var entity = _mapper.Map<Cart>(cartDto);
            try
            {
                await _unitOfWork.Carts.AddAsync(entity);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await _unitOfWork.Carts.Query()
                    .Where(c => c.UserId == cartDto.UserId && c.ProductId == cartDto.ProductId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(c => c.Quantity, c => c.Quantity + cartDto.Quantity));

                await _unitOfWork.CommitAsync();
            }
        }
    }
}
