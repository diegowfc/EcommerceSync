using Application.DTOs.OrderItemDtos;
using Domain.Enums.OrderStatus;

namespace Application.DTOs.OrderDtos
{
    public class OrderUpdateDTO
    {
        public OrderStatus? Status { get; set; }
    }
}
