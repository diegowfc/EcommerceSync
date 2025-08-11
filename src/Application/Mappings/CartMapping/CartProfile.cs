using Application.DTOs.CartDtos;
using AutoMapper;
using Domain.Entities.CartEntity;

namespace Application.Mappings.CartMapping
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartAddDto>().ReverseMap();
        }
    }
}