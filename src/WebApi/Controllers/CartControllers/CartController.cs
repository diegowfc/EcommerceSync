using Application.DTOs.CartDtos;
using Application.Services.CartServices;
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
            var correlationId = await _service.AddItemToCartAsync(cartDto);
            return Accepted(new
            {
                message = "Adicionar item ao carrinho enviado para consumo.",
                correlationId
            });
        }
    }
}