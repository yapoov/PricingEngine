namespace pricing_engine.Promotions;

public static class PromotionFactory
{
    public static List<IPromotion> Create(List<string> keys)
    {
        var promotions = new List<IPromotion>();

        foreach (var key in keys)
        {
            switch (key)
            {
                case "DRESS20":
                    promotions.Add(new CategoryPercentagePromotion(category: "Dress", rate: 0.2m));
                    break;

                case "ACCESSORY_B2G1":
                    promotions.Add(new MultiBuyAccessoryPromotion("Accessories"));
                    break;

                case "SPEND200":
                    promotions.Add(new SpendThresholdPromotion(200,20));
                    break;

                case "BUNDLE":
                    promotions.Add(new BundlePromotion("Tops","Outerwear",15));
                    break;

                case "DANGER10":
                    promotions.Add(new CouponPromotion("DANGER10",0.20m));
                    break;

                case "MEMBER":
                    promotions.Add(new MemberDiscountPromotion(.05m));
                    break;
            }
        }

        return promotions;
    }
}