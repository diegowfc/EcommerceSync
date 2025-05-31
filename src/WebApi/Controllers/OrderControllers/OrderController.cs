using Application.DTOs.OrderDtos;
using Application.Services.OrderServices;
using Domain.Enums.OrderStatus;
using Domain.Interfaces.OrderInterface;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.OrderControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _repository;

        public OrderController(IOrderService service, IOrderRepository repository)
        {
            _orderService = service;
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return Ok(await _orderService.GetOrderByIdAsync(id));
        }

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> UpdateOrderStatus(int id, OrderUpdateDTO dto)
        //{
        //    await _orderService.UpdateOrderStatusAsync(id, dto);
        //    return NoContent();
        //}


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var correlationId = await _orderService.DeleteOrderAsync(id);
            return Accepted(new { CorrelationId = correlationId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDTO dto)
        {
            var correlationId = await _orderService.CreateOrderAsync(dto);
            return Accepted(new { CorrelationId = correlationId });
        }
    }
}
