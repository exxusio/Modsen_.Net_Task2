using BusinessLogicLayer;
using BusinessLogicLayer.Dtos.Products;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken = default)
        {
            var products = await _productService.GetAllProductsAsync(cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
            return Ok(product);
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetProductsByQuery([FromQuery] ProductQuery filter, CancellationToken cancellationToken = default)
        {
            var products = await _productService.GetProductsByFilter(filter, cancellationToken);
            return Ok(products);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto productCreateDto, CancellationToken cancellationToken = default)
        {
            var product = await _productService.CreateProductAsync(productCreateDto, cancellationToken);
            return Ok(product);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductUpdateDto productUpdateDto, CancellationToken cancellationToken = default)
        {
            productUpdateDto.Id = id;
            var product = await _productService.UpdateProductAsync(productUpdateDto, cancellationToken);
            return Ok(product);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedProduct = await _productService.DeleteProductByIdAsync(id, cancellationToken);
            return Ok(deletedProduct);
        }
    }
}
