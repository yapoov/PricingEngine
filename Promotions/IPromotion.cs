using pricing_engine.Pricing;

namespace pricing_engine.Promotions;

public interface IPromotion
{
    IEnumerable<Discount> Apply(PricingContext context);
}