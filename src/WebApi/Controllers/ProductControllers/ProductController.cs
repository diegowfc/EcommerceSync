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
        public async Task<IActionResult> GetAllProducts(int page = 1, int pageSize = 36, CancellationToken cancellationToken = default)
        {
            return Ok(await _productService.GetProductsAsync(page, pageSize, cancellationToken));
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


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _productService.DeleteProductAsync(id);
        //    return NoContent();
        //}
    }
}
