namespace ProductLib.Models.Product;

public readonly record struct ProductId(Guid Value)
{
    public static ProductId CreateNew() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}