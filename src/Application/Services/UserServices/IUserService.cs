using Application.DTOs.UserDtos;

namespace Application.Services.UserServices
{
    public interface IUserService
    {
        Task<Guid> RegisterUser(UserCreateDto userDto);
        Task<UserResponseDTO> GetByIdAsync(int id);
    }
}