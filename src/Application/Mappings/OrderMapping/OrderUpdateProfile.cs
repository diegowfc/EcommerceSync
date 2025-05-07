using Application.DTOs.OrderDtos;
using AutoMapper;
using Domain.Entities.OrderEntity;

namespace Application.Mappings.OrderMapping
{
    public class OrderUpdateProfile : Profile
    {
        public OrderUpdateProfile()
        {
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();
        }     
    }
}
