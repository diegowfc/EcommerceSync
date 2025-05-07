using Application.DTOs.OrderDtos;
using Application.DTOs.ProductDtos;
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

        public async Task<int> CreateOrderAsync(OrderDTO dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new Exception("O pedido deve conter ao menos um item.");

            var order = _mapper.Map<Order>(dto);
            order.DateOfOrder = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.OrderIdentifier = GenerateOrderIdentifier(order.DateOfOrder);
            order.UserId = GenerateUserIdForTest();

            await HandleOrderItem(order);
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();

            return order.Id;
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

        public async Task<IEnumerable<OrderReadDTO>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderReadDTO>>(orders);
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
            var order = await _unitOfWork.Orders.GetByIdAsync(id);

            if (order != null) {
                if (orderUpdateDTO.Status != null)
                    order.Status = (OrderStatus)orderUpdateDTO.Status;

                _unitOfWork.Orders.Update(order);
                await _unitOfWork.CommitAsync();
            } else
            {
                throw new Exception("Pedido não encontrado!");
            }
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

        public async Task HandleOrderItem(Order order)
        {
            float total = 0f;
            var orderItems = new List<OrderItem>();

            foreach (var item in order.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception("Produto não encontrado.");

                total += product.Price * item.Quantity;

                var orderItem = _mapper.Map<OrderItem>(item);
                orderItem.Product = product;

                orderItems.Add(orderItem);
                await HandleProductStock(orderItem);
            }

            order.Total = total;
            order.Items = orderItems;
        }

        public async Task HandleProductStock(OrderItem item)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception("Produto não encontrado.");

            product.Stock -= item.Quantity;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.CommitAsync();
        }
    }
}
