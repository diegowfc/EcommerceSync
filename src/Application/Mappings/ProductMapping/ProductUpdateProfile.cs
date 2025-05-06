using Application.DTOs.ProductDtos;
using AutoMapper;
using Domain.Entities.ProductEntity;

namespace Application.Mappings.ProductMapping
{
    public class ProductUpdateProfile : Profile
    {
        public ProductUpdateProfile()
        {
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
        }
    }
}
