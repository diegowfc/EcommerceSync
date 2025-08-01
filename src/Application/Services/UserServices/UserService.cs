using Application.DTOs.ProductDtos;
using Application.DTOs.UserDtos;
using AutoMapper;
using Domain.Entities.ProductEntity;
using Domain.Entities.UserEntity;
using Domain.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices
{
    public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task RegisterUser(UserCreateDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

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
    }
}
