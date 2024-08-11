using Microsoft.EntityFrameworkCore;
using ProductLib.Models.Product;
using ProductLib.Models.Product.Dto;
using ProductLib.Persistence;
using ProductLib.Services;

namespace ProductLibTest.Services;

public class ProductServiceTest
{
    private readonly DbContextOptions<ProductDbContext> _options;

    public ProductServiceTest()
    {
        _options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        AddTestData();
    }
    
    [Fact]
    public async void Test_GetAllProducts()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var products = await service.GetAllProducts();

        Assert.Equal(_data.Count, products.Count);

        for (var i = 0; i < _data.Count; i++)
        {
            Assert.Equal(_data[i].Id, products[i].Id);
            Assert.Equal(_data[i].Name, products[i].Name);
            Assert.Equal(_data[i].ImgUri, products[i].ImgUri);
            Assert.Equal(_data[i].Price, products[i].Price);
            Assert.Equal(_data[i].Description, products[i].Description);
        }
    }
    
    [Fact]
    public async void Test_GetPage_ZeroPageSize1()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var products = await service.GetPage(page: 0, pageSize: 0);

        Assert.Empty(products);
    }
    
    [Fact]
    public async void Test_GetPage_ZeroPageSize2()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var products = await service.GetPage(page: 1, pageSize: 0);

        Assert.Empty(products);
    }
    
    [Fact]
    public async void Test_GetPage_Case1()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var products = await service.GetPage(page: 0, pageSize: 2);

        Assert.Equal(2, products.Count);

        for (var i = 0; i < 2; i++)
        {
            Assert.Equal(_data[i].Id, products[i].Id);
            Assert.Equal(_data[i].Name, products[i].Name);
            Assert.Equal(_data[i].ImgUri, products[i].ImgUri);
            Assert.Equal(_data[i].Price, products[i].Price);
            Assert.Equal(_data[i].Description, products[i].Description);
        }
    }
    
    [Fact]
    public async void Test_GetPage_Case2()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var products = await service.GetPage(page: 1, pageSize: 2);

        Assert.Single(products);
    }
    
    [Fact]
    public async void Test_GetProductById_Found()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        foreach (var originalProduct in _data)
        {
            var product = await service.GetProductById(originalProduct.Id);
            Assert.NotNull(product);
            Assert.Equal(originalProduct.Id, product.Id);
            Assert.Equal(originalProduct.Name, product.Name);
            Assert.Equal(originalProduct.ImgUri, product.ImgUri);
            Assert.Equal(originalProduct.Price, product.Price);
            Assert.Equal(originalProduct.Description, product.Description);
        }
    }
    
    [Fact]
    public async void Test_GetProductById_NotFound()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var productIds = Enumerable.Range(0, 10).Select(_ => ProductId.CreateNew()).ToList();

        foreach (var productId in productIds)
        {
            var product = await service.GetProductById(productId);
            Assert.Null(product);
        }
    }
    
    [Fact]
    public async void Test_CreateProduct()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var createDto = new CreateProductDto
        {
            Name = "HP ProBook 450 G10",
            ImgUri = "https://image.alza.cz/products/HPBN1068v8n/HPBN1068v8n.jpg?width=1000&height=1000",
            Price = 24989M,
            Description = "Notebook - Intel Core i7 1355U Raptor Lake, 15.6\" IPS antireflexní 1920 \u00d7 1080, RAM 32GB DDR4, Intel UHD Graphics, SSD 1000GB, numerická klávesnice, podsvícená klávesnice, webkamera, USB 3.2 Gen 1, USB-C, čtečka otisků prstů, WiFi 6E, Hmotnost 1,79 kg, Windows 11 Home"
        };

        var productId = await service.CreateProduct(createDto);
        var product = await service.GetProductById(productId);
        
        Assert.NotNull(product);
        Assert.Equal(createDto.Name, product.Name);
        Assert.Equal(createDto.ImgUri, product.ImgUri);
        Assert.Equal(createDto.Price, product.Price);
        Assert.Equal(createDto.Description, product.Description);
    }

    [Fact]
    public async void Test_UpdateProductDescription_Found()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var productId = new ProductId(new Guid("b8fc5ea6-6d6a-415b-b1d5-521a5546e9e1"));
        var updateDto = new UpdateProductDto()
        {
            Description = "very nice"
        };

        var updateResponse = await service.UpdateProductDescription(productId, updateDto);
        var product = await service.GetProductById(productId);
        
        Assert.True(updateResponse.WasFound);
        Assert.NotNull(product);
        Assert.Equal(updateDto.Description, product.Description);
    }
    
    [Fact]
    public async void Test_UpdateProductDescription_NotFound()
    {
        await using var context = new ProductDbContext(_options);
        var service = new ProductService(context);

        var productId = new ProductId(new Guid("b8fc5ea6-6d6a-415b-b1d5-521a5546e9e2"));
        var updateDto = new UpdateProductDto()
        {
            Description = "very nice"
        };

        var updateResponse = await service.UpdateProductDescription(productId, updateDto);
        
        Assert.False(updateResponse.WasFound);
    }
    
    private void AddTestData()
    {
        using var context = new ProductDbContext(_options);
        
        context.Products.AddRange(_data);
        context.SaveChanges();
    }
    
    private readonly List<Product> _data =
    [
        new Product
        {
            Id = new ProductId(new Guid("b8fc5ea6-6d6a-415b-b1d5-521a5546e9e1")),
            Name = "MSI Katana 15 B13VGK-1477CZ",
            ImgUri = "https://image.alza.cz/products/NB200F2l3c/NB200F2l3c.jpg?width=1000&height=1000", 
            Price = 37990M,
            Description =
                "Herní notebook - Intel Core i9 13900H Raptor Lake, 15.6\" IPS matný 2560 \u00d7 1440 165Hz, RAM 16GB DDR5, NVIDIA GeForce RTX 4070 8GB 115 W (MUX Switch), SSD 1000GB, numerická klávesnice, podsvícená RGB klávesnice, webkamera, USB 3.2 Gen 1, USB-C, WiFi 6, WiFi, Bluetooth, hmotnost 2,25 kg, Windows 11 Home"
        },
        new Product
        {
            Id = new ProductId(new Guid("d1cbffe3-096f-4a4e-a5cb-825bf3d97377")),
            Name = "Lenovo ThinkBook 16 G6 IRL Arctic Grey",
            ImgUri = "https://image.alza.cz/products/NT187h31v7a/NT187h31v7a.jpg?width=1000&height=1000", 
            Price = 16990M
        },
        new Product
        {
            Id = new ProductId(new Guid("4558d67b-a18e-4dfb-93c5-a0c3687cea64")),
            Name = "MacBook Pro 14\" M3 PRO CZ 2023 Vesmírně černý",
            ImgUri = "https://image.alza.cz/products/NL258d1a/NL258d1a.jpg?width=1000&height=1000", 
            Price = 57745M,
            Description =
                "MacBook - Apple M3 Pro (11jádrový), 14,2\" IPS lesklý 3024 \u00d7 1964 px, 120 Hz, RAM 18GB, Apple M3 PRO 14jádrová GPU, SSD 512GB, podsvícená klávesnice, webkamera, USB-C, WiFi 6, hmotnost 1,61 kg, macOS"
        }
    ];
}