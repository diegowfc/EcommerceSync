using Application.DTOs.OrderDtos;
using AutoMapper;
using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;

namespace Application.Services.OrderServices
{
    public class OrderService(IMapper mapper) : IOrderService
    {
        private readonly IMapper _mapper = mapper;

        public async Task<int> CreateOrderAsync(OrderDTO dto)
        {
           throw new NotImplementedException();
        }

        public async Task DeleteOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderReadDTO>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<OrderReadDTO> GetOrderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrderAsync(int id, OrderUpdateDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrderStatusAsync(int id, OrderUpdateDTO orderUpdateDTO)
        {
            throw new NotImplementedException();
        }

        public string GenerateOrderIdentifier(DateTime orderDate)
        {
            throw new NotImplementedException();
        }

        public int GenerateUserIdForTest()
        {
            throw new NotImplementedException();

        }

        public async Task HandleOrderItem(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task HandleProductStock(OrderItem item)
        {
            throw new NotImplementedException();
        }
    }
}
