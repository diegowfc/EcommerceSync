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
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentProcessDto dto)
        {
            var result = await _service.ProcessAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new
            {
                message = "Pagamento aprovado",
                transactionId = result.TransactionId
            });
        }
    }
}
