using pricing_engine.Domain;

namespace pricing_engine.Parsing;

public interface ICartParser
{
    Cart Parse(string path);
}