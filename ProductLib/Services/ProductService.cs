using Microsoft.EntityFrameworkCore;
using ProductLib.Models.Product;
using ProductLib.Models.Product.Dto;
using ProductLib.Persistence;

namespace ProductLib.Services;

public class ProductService : IProductService
{
    private readonly ProductDbContext _productDbContext;

    public ProductService(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<List<Product>> GetAllProducts() => await _productDbContext.Products.AsNoTracking().ToListAsync();
    public async Task<List<Product>> GetPage(int? page, int? pageSize)
    {
        var currentPage = page ?? 0;
        var currentPageSize = pageSize ?? 10;
        
        return await _productDbContext.Products.AsNoTracking().Skip(currentPage * currentPageSize).Take(currentPageSize).ToListAsync();
    }

    public async Task<Product?> GetProductById(ProductId productId) => await _productDbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);

    public async Task<ProductId> CreateProduct(CreateProductDto createProductDto)
    {
        var newProductId = ProductId.CreateNew();
        var newProduct = new Product
        {
            Id = newProductId,
            Name = createProductDto.Name,
            ImgUri = createProductDto.ImgUri,
            Price = createProductDto.Price,
            Description = createProductDto.Description
        };

        await _productDbContext.Products.AddAsync(newProduct);
        await _productDbContext.SaveChangesAsync();

        return newProductId;
    }

    public async Task<ProductChangeResultDto> UpdateProductDescription(ProductId productId, UpdateProductDto updateProductDto)
    {
        var product = await _productDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
        
        if (product == null)
            return new ProductChangeResultDto { WasFound = false };

        product.Description = updateProductDto.Description;
        
        await _productDbContext.SaveChangesAsync();

        return new ProductChangeResultDto { WasFound = true };
    }

    public async Task<ProductChangeResultDto> DeleteProduct(ProductId productId)
    {
        var product = await _productDbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
            return new ProductChangeResultDto { WasFound = false };

        _productDbContext.Products.Remove(product);
        await _productDbContext.SaveChangesAsync();

        return new ProductChangeResultDto { WasFound = true };
    }
}