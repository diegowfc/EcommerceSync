using Application.DTOs.CartDtos;
using Application.Services.CartServices;
using Application.Services.UserServices;
using Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.CartControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService service) : ControllerBase
    {
        private readonly ICartService _service = service;

        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartAddDto cartDto)
        {
            await _service.AddItemToCartAsync(cartDto);
            return Ok(new { message = "Item adicionado ao carrinho" });
        }
    }
}
