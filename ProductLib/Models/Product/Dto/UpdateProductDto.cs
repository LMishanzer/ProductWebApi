using System.ComponentModel.DataAnnotations;

namespace ProductLib.Models.Product.Dto;

public class UpdateProductDto
{
    [StringLength(ProductConstants.MaxDescriptionLength)]
    [Required]
    public required string Description { get; set; }
}