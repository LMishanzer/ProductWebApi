using Microsoft.AspNetCore.Mvc;
using ProductLib.Models.Product;
using ProductLib.Models.Product.Dto;
using ProductLib.Services;

namespace ProductWebApi.Controllers;

[Route("/api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var allProducts = await _productService.GetAllProducts();
        var allDtoProducts = allProducts.Select(p => new ProductDto(p));

        return Ok(allDtoProducts);
    }
    
    [HttpGet("/api/v2/[controller]")]
    public async Task<IActionResult> GetPage([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var allProducts = await _productService.GetPage(page, pageSize);
        var allDtoProducts = allProducts.Select(p => new ProductDto(p));

        return Ok(allDtoProducts);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var productId = new ProductId(id);
        var product = await _productService.GetProductById(productId);

        return product != null ? Ok(new ProductDto(product)) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var productId = await _productService.CreateProduct(productDto);
        return Created(productId.Value.ToString(), new { Id = productId.Value });
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateDescription([FromRoute] Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var productId = new ProductId(id);
        var result = await _productService.UpdateProductDescription(productId, updateProductDto);

        return result.WasFound ? NoContent() : NotFound();
    }
}