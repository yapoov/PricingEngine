using pricing_engine.Domain;
using pricing_engine.Pricing;

namespace pricing_engine.Promotions;

public class BundlePromotion : IPromotion
{
    private readonly string _categoryA;
    private readonly string _categoryB;
    private readonly decimal _discountPerPair;

    public BundlePromotion(string categoryA, string categoryB, decimal discountPerPair)
    {
        _categoryA = categoryA;
        _categoryB = categoryB;
        _discountPerPair = discountPerPair;
    }

    public IEnumerable<Discount> Apply(PricingContext context)
    {
        var groups = new Dictionary<string, int>();

        foreach (var line in context.Cart.Lines)
        {
            var product = context.Catalog.Get(line.Sku);

            groups.TryAdd(product.Category, 0);
            groups[product.Category] += line.Quantity;
        }

        if (!groups.TryGetValue(_categoryA, out var countA))
            countA = 0;

        if (!groups.TryGetValue(_categoryB, out var countB))
            countB = 0;

        var pairs = Math.Min(countA, countB);

        if (pairs <= 0)
            yield break;

        yield return new Discount(
            $"Bundle: {_categoryA} + {_categoryB} ({_discountPerPair:0.00} per pair)",
            pairs * _discountPerPair
        );
    }
}