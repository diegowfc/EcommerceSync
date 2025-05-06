using Application.DTOs.ProductDtos;
using AutoMapper;
using Domain.Entities.ProductEntity;

namespace Application.Mappings.ProductMapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
