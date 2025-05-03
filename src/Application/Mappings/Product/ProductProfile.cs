using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities.ProductEntity;

namespace Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
