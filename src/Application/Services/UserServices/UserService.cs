using Application.DTOs.UserDtos;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities.UserEntity;
using Domain.Interfaces.UnitOfWork;
using System.Security.Cryptography;

namespace Application.Services.UserServices
{
    public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task RegisterUser(UserCreateDto userDto, CancellationToken ct = default)
        {
            var user = _mapper.Map<User>(userDto);

            if (user is null) throw new ArgumentNullException(nameof(user));

            var normalizedEmail = NormalizeEmail(userDto.Email);
            if (string.IsNullOrWhiteSpace(normalizedEmail))
            {
                throw new ArgumentException("E-mail inválido.", nameof(userDto.Email));
            }

            if (await _unitOfWork.Users.ExistsByEmailAsync(normalizedEmail, ct).ConfigureAwait(false))
                throw new EmailAlreadyExistsException("Já existe um usuário cadastrado com esse e-mail.");

            var salt = GenerateSalt();
            var hash = HashPassword(userDto.Password, salt);

            user.PasswordHash = $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();
        }

        private static byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        private static byte[] HashPassword(string password, byte[] salt,
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

        public async Task<UserResponseDTO> GetByIdAsync(int id)
        {
            var userEntity = await _unitOfWork.Users.GetUserByIdAsync(id);

            return _mapper.Map<UserResponseDTO>(userEntity);
        }

        private static string NormalizeEmail(string email)
        {
            return email?.Trim().ToLowerInvariant();
        }
    }
}
