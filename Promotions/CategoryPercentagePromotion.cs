using pricing_engine.Pricing;

namespace pricing_engine.Promotions;

public class CategoryPercentagePromotion : IPromotion
{
    private readonly string _category;
    private readonly decimal _rate;

    public CategoryPercentagePromotion(string category, decimal rate)
    {
        _category = category;
        _rate = rate;
    }

    public IEnumerable<Discount> Apply(PricingContext context)
    {
        decimal totalDiscount = 0;

        foreach (var line in context.Cart.Lines)
        {
            var product = context.Catalog.Get(line.Sku);

            if (product.Category != _category)
                continue;

            totalDiscount += product.Price * line.Quantity * _rate;
        }
        if(totalDiscount<=0)
            yield break;

        yield return new Discount($"{_rate:P0} off {_category}", decimal.Round(totalDiscount, 2));
        
    }
}