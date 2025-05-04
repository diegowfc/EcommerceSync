using Application.DTOs;
using AutoMapper;
using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Enums.OrderStatus;
using Domain.Interfaces.UnitOfWork;

namespace Application.Services.OrderServices
{
    public class OrderService(IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task CreateOrderAsync(OrderDTO dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new Exception("O pedido deve conter ao menos um item.");

            var order = _mapper.Map<Order>(dto);
            order.DateOfOrder = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;

            float total = 0f;
            var orderItems = new List<OrderItem>();

            foreach (var item in dto.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception("Produto não encontrado.");

                total += product.Price * item.Quantity;

                var orderItem = _mapper.Map<OrderItem>(item);
                orderItem.Product = product;

                orderItems.Add(orderItem);
            }

            order.Total = total;
            order.Items = orderItems;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);

            if (order != null) {
                _unitOfWork.Orders.Remove(order);
                await _unitOfWork.CommitAsync();
            } else {
                throw new Exception("Pedido não encontrado!");
            }
                
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task UpdateOrderAsync(int id, OrderDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrderStatusAsync(int id, OrderDTO dto)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);

            if (order != null) {
                order.Status = dto.Status;

                _unitOfWork.Orders.Update(order);
                await _unitOfWork.CommitAsync();
            } else
            {
                throw new Exception("Pedido não encontrado!");
            }
        }
    }
}
