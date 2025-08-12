using Application.DTOs.PaymentDtos;
using Application.Services.PaymentServices;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.PaymentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _service = paymentService;

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment(
            [FromBody] PaymentProcessDto paymentDto,
            [FromServices] FakePaymentGatewayClient fakeGateway,
            [FromQuery] bool simulateDown = false)
        {
            fakeGateway.IsAvailable = !simulateDown;

            try
            {
                var correlationId = await _service.ProcessPaymentAsync(paymentDto);

                return Accepted(new
                {
                    message = "Processar pagamento enviado para consumo.",
                    correlationId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new { message = ex.Message });
            }
        }
    }
}
