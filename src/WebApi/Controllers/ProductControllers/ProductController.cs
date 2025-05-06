using Application.DTOs.ProductDtos;
using Application.Services.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _service.GetAllProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(await _service.GetProductByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            var createdProduct = await _service.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct }, dto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, ProductUpdateDTO dto)
        {
            await _service.UpdateProductAsync(id, dto);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
