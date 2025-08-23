using Application.DTOs.UserDtos;

namespace Application.Services.UserServices
{
    public interface IUserService
    {
        Task<Guid> RegisterUser(UserCreateDto userDto, CancellationToken ct = default);
        Task<UserResponseDTO> GetByIdAsync(int id);
    }
}