using Application.DTOs.PagedResultsDTO;
using Application.DTOs.ProductDtos;
using Application.DTOs.UserDtos;

namespace Application.Services.UserServices
{
    public interface IUserService
    {
        Task RegisterUser(UserCreateDto userDto);
        Task<UserResponseDTO> GetByIdAsync(int id);

    }
}
