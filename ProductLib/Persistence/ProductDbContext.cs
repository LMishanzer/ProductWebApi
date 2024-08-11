using Microsoft.EntityFrameworkCore;
using ProductLib.Models.Product;

namespace ProductLib.Persistence;

public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public ProductDbContext(DbContextOptions options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.ImgUri).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired();

        modelBuilder.Entity<Product>().Property(p => p.Id).HasConversion(id => id.Value, value => new ProductId(value));
    }
}