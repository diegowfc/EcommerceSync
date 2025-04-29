using Application.DTOs;
using AutoMapper;
using Domain.Entities.OrderItem;

namespace Application.Mappings
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        }
    }
}
