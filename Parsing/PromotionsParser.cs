using pricing_engine.Promotions;

namespace pricing_engine.Parsing;

public class PromotionParser : IPromotionParser
{
    public List<IPromotion> Parse(string path)
    {
        var lines = File.ReadAllLines(path)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Where(l => !l.StartsWith("#"));

        return PromotionFactory.Create(lines.ToList());
    }
}