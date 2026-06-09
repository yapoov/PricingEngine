namespace pricing_engine.Domain;

public record Cart(List<CartLine> Lines);
public record CartLine(string Sku, int Quantity);