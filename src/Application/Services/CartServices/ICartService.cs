using Application.DTOs.CartDtos;
using Application.DTOs.OrderDtos;

namespace Application.Services.CartServices
{
    public interface ICartService
    {
        Task AddItemToCartAsync(CartAddDto cartDto);
    }
}