using pricing_engine.Pricing;

namespace pricing_engine.Promotions;

public class MultiBuyAccessoryPromotion : IPromotion
{
    private readonly string _category;

    public MultiBuyAccessoryPromotion(string category)
    {
        _category = category;
    }


    public IEnumerable<Discount> Apply(PricingContext context)
    {
        var unitPrices = new List<decimal>();
        foreach (var line in context.Cart.Lines)
        {
            var product = context.Catalog.Get(line.Sku);

            if (product.Category != _category)
                continue;

            for (int i = 0; i < line.Quantity; i++)
            {
                unitPrices.Add(product.Price);
            }
        }

        if (unitPrices.Count < 3)
            yield break;

        unitPrices.Sort((a, b) => b.CompareTo(a));
        decimal totalDiscount = 0;
        for (int i = 0; i + 2 < unitPrices.Count; i += 3)
        {
            // The cheapest item in a 3-unit group is always the 3rd item when sorted descending
            totalDiscount += unitPrices[i + 2];
        }
        if (totalDiscount > 0)
        {
            yield return new Discount($"Buy 2 Get 1 Free ({_category})", decimal.Round(totalDiscount, 2));
        }
    }
}