using Application.DTOs.PagedResultsDTO;
using Application.DTOs.ProductDtos;
using Application.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService service)
        {
            _productService = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CursorPage<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts( [FromQuery] int? afterId = null, [FromQuery] int pageSize = 36, CancellationToken cancellationToken = default)
        {
            var result = await _productService.GetProductsAsync(afterId, pageSize, cancellationToken);

            if (result.HasMore && result.NextAfter.HasValue)
            {
                var nextUrl = Url.ActionLink(nameof(GetAllProducts), values: new { afterId = result.NextAfter, pageSize });
                Response.Headers.Append("Link", $"<{nextUrl}>; rel=\"next\"");
                Response.Headers.Append("X-Next-After", result.NextAfter.Value.ToString());
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO productDto)
        {
            var correlationID = await _productService.CreateProductAsync(productDto);
            return Accepted(new { CorrelationId = correlationID });
        }

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> Update(int id, ProductUpdateDTO dto)
        //{
        //    await _productService.UpdateProductAsync(id, dto);
        //    return NoContent();
        //}


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var correlationID = await _productService.DeleteProductAsync(id);
            return Accepted(new { CorrelationId = correlationID });
        }
    }
}
