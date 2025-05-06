using Application.DTOs.OrderItemDtos;
using AutoMapper;
using Domain.Entities.OrderItemEntity;

namespace Application.Mappings.OrderItemMapping
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        }
    }
}
