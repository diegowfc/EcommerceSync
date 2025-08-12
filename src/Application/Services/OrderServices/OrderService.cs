using Application.DTOs.OrderDtos;
using Application.DTOs.ProductDtos;
using AutoMapper;
using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Enums.OrderStatus;
using Domain.Event;
using Domain.Interfaces.UnitOfWork;
using MassTransit;

namespace Application.Services.OrderServices
{
    public class OrderService(
        ISendEndpointProvider sendEndpointProvider,
        IPublishEndpoint publishEndpoint,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IOrderService
    {
        private readonly ISendEndpointProvider _send = sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<OrderCreatedResponseDTO> CreateOrderAsync(OrderDTO orderDTO)
        {
            if (orderDTO.Items == null || !orderDTO.Items.Any())
                throw new Exception("O pedido deve conter ao menos um item.");

            var correlationId = Guid.NewGuid();

            var dateOfOrder = DateTime.UtcNow;
            var orderIdentifier = GenerateOrderIdentifier(dateOfOrder);
            var userId = GenerateUserIdForTest();
            var status = OrderStatus.Pending;

            var itemsForEvent = orderDTO.Items.Select(i => new OrderItemCreatedEvent
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList();

            var orderRegister = new OrderRegistrationCommand
            {
                CorrelationId = correlationId,
                DateOfOrder = dateOfOrder,
                OrderIdentifier = orderIdentifier,
                UserId = userId,
                Status = status,
                Items = itemsForEvent
            };

            var endpoint = await _send.GetSendEndpoint(new Uri("queue:order-registration-commands"));
            await endpoint.Send(orderRegister);

            var orderCreatedResponse = new OrderCreatedResponseDTO
            {
                CorrelationId = correlationId,
                Message = $"Evento de criação do pedido iniciado (ID: {correlationId})"
            };

            return orderCreatedResponse;

        }

        public async Task<OrderDeletedResponseDTO> DeleteOrderAsync(int id)
        {
            var exists = await _unitOfWork.Orders.GetByIdAsync(id);

            if (exists is null)
                throw new Exception($"Pedido com ID {id} não encontrado.");

            var correlationId = Guid.NewGuid();

            await _publishEndpoint.Publish(new OrderDeletedEvent
            {
                CorrelationID = correlationId,
                OrderId = id
            });

            var orderDeletedResponse = new OrderDeletedResponseDTO
            {
                CorrelationId = correlationId,
                Message = $"Evento de delete do pedido iniciado (ID: {correlationId})"
            };

            return orderDeletedResponse;
        }

        public async Task<OrderReadDTO> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            return _mapper.Map<OrderReadDTO>(order);
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
            string prefix = "USP";
            string datePart = orderDate.ToString("yyyyMMdd");
            string randomSequence = new Random().Next(1, 10000).ToString("D4");

            return $"{prefix}-{datePart}-{randomSequence}";
        }

        public int GenerateUserIdForTest()
        {
            var random = new Random();
            return random.Next(1, 2);
        }
    }
}
