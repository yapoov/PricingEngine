using pricing_engine.Domain;

namespace pricing_engine.Parsing;

public class CartFileParser : ICartParser
{
    public Cart Parse(string path)
    {
        var lines = File.ReadAllLines(path)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Where(l => !l.StartsWith("#"));

        var cartLines = new List<CartLine>();

        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split('|', StringSplitOptions.TrimEntries);

            cartLines.Add(new CartLine(
                parts[0],
                int.Parse(parts[1])
            ));
        }

        return new Cart(cartLines);
    }
}