using Application.DTOs.OrderDtos;
using Application.DTOs.ProductDtos;
using AutoMapper;
using Domain.Entities.OrderEntity;
using Domain.Entities.OrderItemEntity;
using Domain.Enums.OrderStatus;
using Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(OrderDTO dto)
        {
            if (dto.Items is null || !dto.Items.Any())
                throw new ArgumentException("O pedido deve conter ao menos um item.");

            var now = DateTime.UtcNow;

            var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToArray();

            // Catálogo com preço (e o que mais precisar), SEM rastreamento
            var catalog = await _unitOfWork.Products.Query()
                .AsNoTracking()
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Price })
                .ToDictionaryAsync(p => p.Id);

            var faltantes = productIds.Except(catalog.Keys).ToArray();
            if (faltantes.Length > 0)
                throw new InvalidOperationException($"Produtos inexistentes: {string.Join(",", faltantes)}");

            var qtyByProduct = dto.Items
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            foreach (var (productId, qty) in qtyByProduct)
            {
                var affected = await _unitOfWork.Products.Query()
                    .Where(p => p.Id == productId && p.Stock >= qty)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Stock, p => p.Stock - qty));

                if (affected == 0)
                    throw new InvalidOperationException($"Estoque insuficiente para o produto {productId}.");
            }

            var items = dto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList();

            decimal total = dto.Items.Sum(i => (decimal)catalog[i.ProductId].Price * i.Quantity);

            var order = new Order
            {
                DateOfOrder = now,
                Status = OrderStatus.Pending,
                OrderIdentifier = GenerateOrderIdentifier(now),
                UserId = GenerateUserIdForTest(),
                Items = items,
                Total = (float)total
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();


            return order.Id;
        }


        public async Task DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                throw new Exception("Pedido não encontrado!");

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.CommitAsync();
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
            if (order == null)
                throw new Exception("Pedido não encontrado!");

            if (orderUpdateDTO.Status != null)
                order.Status = (OrderStatus)orderUpdateDTO.Status;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CommitAsync();
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
