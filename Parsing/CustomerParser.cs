using pricing_engine.Domain;

namespace pricing_engine.Parsing;

public class CustomerParser : ICustomerParser
{
    public CustomerContext Parse(string path)
    {
        var raw = File.ReadAllText(path);

        bool isMember = false;
        string? coupon = null;

        var parts = raw.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var kv = part.Split(':', 2);

            if (kv.Length != 2)
                continue;

            var key = kv[0].Trim().ToLower();
            var value = kv[1].Trim();

            if (key == "member")
                isMember = value.Equals("true", StringComparison.OrdinalIgnoreCase);

            if (key == "coupon")
                coupon = value;
        }

        return new CustomerContext(isMember, coupon);
    }
}