using pricing_engine.Pricing;

namespace pricing_engine.Promotions;

public class MemberDiscountPromotion : IPromotion
{
    private readonly decimal _rate;
    public MemberDiscountPromotion(decimal rate)
    {
        _rate = rate;
    }
    public IEnumerable<Discount> Apply(PricingContext context)
    {
        if (!context.Customer.IsMember) yield break;
        yield return new Discount($"Member discount {_rate:P0} OFF", decimal.Round(context.CurrentSubtotal * _rate, 2));
    }
}