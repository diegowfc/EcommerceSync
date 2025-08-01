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
        private readonly IOrderService _service;
        private readonly IOrderRepository _repository;

        public OrderController(IOrderService service, IOrderRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            return Ok(await _service.GetOrderByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteOrderAsync(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDTO dto)
        {
            var createdOrder = await _service.CreateOrderAsync(dto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder }, dto);
        }
    }
}
