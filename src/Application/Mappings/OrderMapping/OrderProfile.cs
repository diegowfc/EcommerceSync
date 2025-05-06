using Application.DTOs.OrderDtos;
using AutoMapper;
using Domain.Entities.OrderEntity;

namespace Application.Mappings.OrderMapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
