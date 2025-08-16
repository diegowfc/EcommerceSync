using Application.DTOs.PagedResultsDTO;
using Application.DTOs.ProductDtos;
using Application.Services.ProductServices;
using Domain.Interfaces.ProductInterface;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IProductRepository _repository;

        public ProductController(IProductService service, IProductRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CursorPage<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts([FromQuery] int? afterId = null, [FromQuery] int pageSize = 36, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetProductsAsync(afterId, pageSize, cancellationToken);

            if (result.HasMore && result.NextAfter.HasValue)
            {
                var nextUrl = Url.ActionLink(nameof(GetAllProducts), values: new { afterId = result.NextAfter, pageSize });
                Response.Headers.Append("Link", $"<{nextUrl}>; rel=\"next\"");
                Response.Headers.Append("X-Next-After", result.NextAfter.Value.ToString());
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(await _repository.GetProductByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            var createdProduct = await _service.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
