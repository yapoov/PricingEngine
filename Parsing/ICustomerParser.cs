using pricing_engine.Domain;

namespace pricing_engine.Parsing;

public interface ICustomerParser
{
    public CustomerContext Parse(string path);
}