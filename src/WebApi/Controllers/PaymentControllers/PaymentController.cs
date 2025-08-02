using Application.DTOs.PaymentDtos;
using Application.DTOs.UserDtos;
using Application.Services.PaymentServices;
using Application.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.PaymentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService service) : ControllerBase
    {
        private readonly IPaymentService _service = service;

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment(
            [FromBody] PaymentProcessDto dto, 
            [FromServices] FakePaymentGatewayClient fakeGateway,
            [FromQuery] bool simulateDown = false)
        {
            fakeGateway.IsAvailable = !simulateDown;

            try
            {
                var result = await _service.ProcessAsync(dto);

                return Ok(new { message = "Pagamento aprovado!", transactionId = result.TransactionId });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new { message = ex.Message });
            }
        }
    }
}
