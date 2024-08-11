using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductLib.Models.Product;

[Table("Products")]
public class Product
{
    [Key]
    public ProductId Id { get; init; }
    
    [MaxLength(ProductConstants.MaxNameLength)]
    [Required]
    public required string Name { get; init; }
    
    [MaxLength(ProductConstants.MaxImgUriLength)]
    [Required]
    public required string ImgUri { get; init; }
    
    [Required]
    public required decimal Price { get; init; }
    
    [MaxLength(ProductConstants.MaxDescriptionLength)]
    public string? Description { get; set; }
}