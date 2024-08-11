using ProductLib.Models.Product;

namespace ProductLibTest.Models;

public class ProductIdTest
{
    [Fact]
    public void Test_ProductIdConsistency()
    {
        const string guidString = "23c45654-86b6-484e-a6d6-a0e7cd7f5efe";
        var guid = new Guid(guidString);
        var productId = new ProductId(guid);
        
        Assert.Equal(guid, productId.Value);
        Assert.Equal(guidString, productId.ToString());
    }
}