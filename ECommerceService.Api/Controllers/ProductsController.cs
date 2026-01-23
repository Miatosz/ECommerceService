using ECommerceService.Api.Dto;
using ECommerceService.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace ECommerceService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {       
            var product = await _productService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetProductById), new { id = product }, product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<GetProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<GetProductDto> GetWithQuery()
        {
            return _productService.QueryProducts();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto dto)
        {
            var product = await _productService.UpdateProduct(dto);

            return Ok(product);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
