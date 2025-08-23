using Application.DTOs.ProductDtos;
using Application.DTOs.UserDtos;
using Application.Services.OrderServices;
using Application.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

        [HttpPost("register")]
        [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(UserCreateDto userDto)
        {
            await _service.RegisterUser(userDto);
            return StatusCode(201, new { message = "Usuário criado com sucesso" });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserInformation([FromRoute] int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"Usuário {id} não encontrado." });

            return Ok(user);
        }
    }
}
