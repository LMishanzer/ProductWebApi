using ProductLib.Models.Product;
using ProductLib.Models.Product.Dto;

namespace ProductLibTest.Models.Dto;

public class ProductDtoTest
{
    [Fact]
    public void Test_ProductDtoCreation()
    {
        var product = new Product
        {
            Id = new ProductId(new Guid("4558d67b-a18e-4dfb-93c5-a0c3687cea64")),
            Name = "MacBook Pro 14\" M3 PRO CZ 2023 Vesmírně černý",
            ImgUri = "https://image.alza.cz/products/NL258d1a/NL258d1a.jpg?width=1000&height=1000",
            Price = 57745M,
            Description =
                "MacBook - Apple M3 Pro (11jádrový), 14,2\" IPS lesklý 3024 \u00d7 1964 px, 120 Hz, RAM 18GB, Apple M3 PRO 14jádrová GPU, SSD 512GB, podsvícená klávesnice, webkamera, USB-C, WiFi 6, hmotnost 1,61 kg, macOS"
        };

        var productDto = new ProductDto(product);
        
        Assert.Equal(product.Id.Value, productDto.Id);
        Assert.Equal(product.Name, productDto.Name);
        Assert.Equal(product.ImgUri, productDto.ImgUri);
        Assert.Equal(product.Price, productDto.Price);
        Assert.Equal(product.Description, productDto.Description);
    }
}