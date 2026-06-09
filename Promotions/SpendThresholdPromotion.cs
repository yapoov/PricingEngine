using pricing_engine.Pricing;

namespace pricing_engine.Promotions;

public class SpendThresholdPromotion
    : IPromotion
{
    public SpendThresholdPromotion(decimal threshold, decimal discountAmount)
    {
        _threshold = threshold;
        _discountAmount = discountAmount;
    }

    private readonly decimal _threshold;
    private readonly decimal _discountAmount;

    public IEnumerable<Discount> Apply(PricingContext context)
    {
        if (context.BaseSubtotal < _threshold)
            yield break;
        yield return new Discount($"Spend more than {_threshold:C} then {_discountAmount:C} off", _discountAmount);
    }
}