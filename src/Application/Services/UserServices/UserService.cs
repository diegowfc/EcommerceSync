using Application.DTOs.OrderDtos;
using Application.DTOs.UserDtos;
using AutoMapper;
using Domain.Entities.UserEntity;
using Domain.Event;
using Domain.Interfaces.EndpointCache;
using Domain.Interfaces.UnitOfWork;
using MassTransit;
using MassTransit.Transports;
using System.Security.Cryptography;

namespace Application.Services.UserServices
{
    public class UserService(
        IEndpointCache endpointCache,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IUserService
    {

        private static readonly string QueueUserRegistration = "user-registration-commands";
        private readonly Task<ISendEndpoint> _userRegistrationEndpoint = endpointCache.ForQueue(QueueUserRegistration);
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserResponseDTO> GetByIdAsync(int id)
        {
            var userEntity = await _unitOfWork.Users.GetUserByIdAsync(id);
            return _mapper.Map<UserResponseDTO>(userEntity);
        }

        public async Task<Guid> RegisterUser(UserCreateDto userDto)
        {
            var correlationId = NewId.NextGuid();

            var salt = GenerateSalt();
            var hash = HashPassword(userDto.Password, salt);

            var userRegister = new UserRegisterCommand
            {
                CorrelationId = correlationId,
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}"
            };

            var endpoint = await _userRegistrationEndpoint.ConfigureAwait(false);
            await endpoint.Send(userRegister).ConfigureAwait(false);

            return correlationId;
        }

        private static byte[] HashPassword(
            string password, byte[] salt,
            int iterations = 10000,
            int hashSize = 32)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256
            );
            return pbkdf2.GetBytes(hashSize);
        }

        private static byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
    }
}
