using Application.DTOs.UserDtos;
using Application.Services.UserServices;
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
            var correlationId = await _service.RegisterUser(userDto);
            return Accepted(new
            {
                message = "Usuário enviado para criação.",
                correlationId
            });
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