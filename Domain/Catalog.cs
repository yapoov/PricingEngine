namespace pricing_engine.Domain;

public record Product(string Sku, string Name, string Category, decimal Price);

public class Catalog
{
    private readonly Dictionary<string, Product> _productDictionary = new();
    public Catalog(IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            _productDictionary[product.Sku] = product;
        }
    }
    public Product Get(string lineSku)
    {
        return _productDictionary[lineSku];
    }
}