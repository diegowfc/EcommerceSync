using Application.DTOs;
using AutoMapper;
using Domain.Entities.OrderEntity;

namespace Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
