using pricing_engine.Pricing;

namespace pricing_engine.Promotions;

public class CouponPromotion : IPromotion
{
    private readonly decimal _rate;
    private readonly string _couponCode;

    public CouponPromotion(string couponCode, decimal rate)
    {
        _rate = rate;
        _couponCode = couponCode;
    }

    public IEnumerable<Discount> Apply(PricingContext context)
    {
        if (context.Customer.CouponCode == _couponCode)
            yield return new Discount($"Coupon code {_couponCode} Applied {_rate:P0} OFF", decimal.Round(context.CurrentSubtotal * _rate, 2));
    }
}