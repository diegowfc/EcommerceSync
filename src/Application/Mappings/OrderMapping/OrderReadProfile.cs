using Application.DTOs.OrderDtos;
using AutoMapper;
using Domain.Entities.OrderEntity;

namespace Application.Mappings.OrderMapping
{
    public class OrderReadProfile : Profile
    {
        public OrderReadProfile()
        {
            CreateMap<Order, OrderReadDTO>().ReverseMap();
        }
    }
}
