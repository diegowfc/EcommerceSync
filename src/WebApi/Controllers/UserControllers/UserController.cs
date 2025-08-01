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
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
            //_repository = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserCreateDto userDto)
        {
            await _service.RegisterUser(userDto);
            return StatusCode(201, new { message = "Usuário criado com sucesso" });
        }
    }
}
