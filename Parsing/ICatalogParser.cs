using pricing_engine.Domain;

namespace pricing_engine.Parsing;

public interface ICatalogParser
{
    Catalog Parse(string path);
}