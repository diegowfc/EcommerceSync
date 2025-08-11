using Application.DTOs.UserDtos;
using AutoMapper;
using Domain.Entities.UserEntity;


namespace Application.Mappings.UserMapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserCreateDto>().ReverseMap();
        }
    }
}