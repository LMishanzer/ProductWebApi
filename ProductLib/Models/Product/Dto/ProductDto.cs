namespace ProductLib.Models.Product.Dto;

public class ProductDto
{
    public Guid Id { get; }
    
    public string Name { get; }
    
    public string ImgUri { get; }
    
    public decimal Price { get; }
    
    public string? Description { get; }
    
    public ProductDto(Product product)
    {
        Id = product.Id.Value;
        Name = product.Name;
        ImgUri = product.ImgUri;
        Price = product.Price;
        Description = product.Description;
    }
}