using System.ComponentModel.DataAnnotations;

namespace ProductLib.Models.Product.Dto;

public class CreateProductDto
{
    [Required]
    [StringLength(ProductConstants.MaxNameLength)]
    public required string Name { get; set; }
    
    [Required]
    [StringLength(ProductConstants.MaxImgUriLength)]
    [Url]
    public required string ImgUri { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    [StringLength(ProductConstants.MaxDescriptionLength)]
    public string? Description { get; set; }
}