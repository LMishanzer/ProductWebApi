using ProductLib.Models.Product;
using ProductLib.Models.Product.Dto;

namespace ProductLib.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProducts();
    Task<List<Product>> GetPage(int? page, int? pageSize);
    Task<Product?> GetProductById(ProductId id);
    Task<ProductId> CreateProduct(CreateProductDto createProductDto);
    Task<ProductChangeResultDto> UpdateProductDescription(ProductId productId, UpdateProductDto updateProductDto);
}