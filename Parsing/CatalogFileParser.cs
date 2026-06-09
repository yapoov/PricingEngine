using pricing_engine.Domain;


namespace pricing_engine.Parsing;

public class CatalogFileParser : ICatalogParser
{
    public Catalog Parse(string path)
    {
        var lines = File.ReadAllLines(path)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Where(l => !l.StartsWith("#"));

        var products = new List<Product>();

        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split('|', StringSplitOptions.TrimEntries).Skip(1).ToArray();

            products.Add(new Product(
                parts[0],
                parts[1],
                parts[2],
                decimal.Parse(parts[3])
            ));
        }

        return new Catalog(products);
    }
}