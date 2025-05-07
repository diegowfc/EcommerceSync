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
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _service.GetAllProductsAsync());
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
