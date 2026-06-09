using pricing_engine.Promotions;

namespace pricing_engine.Parsing;

public interface IPromotionParser
{
    public List<IPromotion> Parse(string path);
}