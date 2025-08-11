using Application.DTOs.UserDtos;
using AutoMapper;
using Domain.Entities.UserEntity;


namespace Application.Mappings.UserMapping
{
    public class UserReadProfile : Profile
    {
        public UserReadProfile()
        {
            CreateMap<User, UserResponseDTO>().ReverseMap();
        }
    }
}