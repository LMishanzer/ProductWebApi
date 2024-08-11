using Microsoft.AspNetCore.Mvc;
using ProductLib.Models.Product;
using ProductLib.Models.Product.Dto;
using ProductLib.Services;

namespace ProductWebApi.Controllers;

/// <summary>
/// Controller for handling product-related operations.
/// </summary>
[Route("/api/v1/products")]
[Consumes("application/json")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    
    /// <inheritdoc />
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Retrieves a list of all products.
    /// </summary>
    /// <returns>A list of products</returns>
    [HttpGet]
    [ProducesResponseType<List<ProductDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var allProducts = await _productService.GetAllProducts();
        var allDtoProducts = allProducts.Select(p => new ProductDto(p));

        return Ok(allDtoProducts);
    }

    /// <summary>
    /// Retrieves a page of products.
    /// </summary>
    /// <param name="page">The page number</param>
    /// <param name="pageSize">The number of products per page</param>
    /// <returns>A list of products</returns>
    [HttpGet("/api/v2/products")]
    [ProducesResponseType<List<ProductDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPage([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var allProducts = await _productService.GetPage(page, pageSize);
        var allDtoProducts = allProducts.Select(p => new ProductDto(p));

        return Ok(allDtoProducts);
    }

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product</param>
    /// <returns>The product with the specified ID, or null if not found</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var productId = new ProductId(id);
        var product = await _productService.GetProductById(productId);

        return product != null ? Ok(new ProductDto(product)) : NotFound();
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productDto">The data for the new product</param>
    /// <returns>The ID of the created product</returns>
    [HttpPost]
    [ProducesResponseType<ProductDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var productId = await _productService.CreateProduct(productDto);
        return Created(productId.Value.ToString(), new { Id = productId.Value });
    }

    /// <summary>
    /// Updates the description of a product.
    /// </summary>
    /// <param name="id">The ID of the product</param>
    /// <param name="updateProductDto">The updated product description</param>
    /// <returns>Returns the HTTP status code 204 No Content if the product was found and the description was updated,
    /// or returns the HTTP status code 404 Not Found if the product was not found</returns>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDescription([FromRoute] Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var productId = new ProductId(id);
        var result = await _productService.UpdateProductDescription(productId, updateProductDto);

        return result.WasFound ? NoContent() : NotFound();
    }
}