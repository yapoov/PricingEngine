using pricing_engine.Domain;
using pricing_engine.Promotions;

namespace pricing_engine.Pricing;

public class PricingContext
{
    public Catalog Catalog { get; }
    public Cart Cart { get; }
    public CustomerContext Customer { get; }
    
    public decimal BaseSubtotal { get; }
    public decimal CurrentSubtotal { get; private set; }
    
    private readonly List<Discount> _appliedDiscounts = new();
    public IReadOnlyCollection<Discount> AppliedDiscounts => _appliedDiscounts;
    
    public PricingContext(Catalog catalog, Cart cart, CustomerContext customer)
    {
        Catalog = catalog;
        Cart = cart;
        Customer = customer;

        BaseSubtotal = cart.Lines
            .Sum(l => catalog.Get(l.Sku).Price * l.Quantity);
        CurrentSubtotal = BaseSubtotal;
    }
    
    public void AddDiscount(Discount discount)
    {
        _appliedDiscounts.Add(discount);
        CurrentSubtotal = Math.Max(0, CurrentSubtotal - discount.Amount);
    }
    
    public int GetQuantity(string sku)
    {
        return Cart.Lines.FirstOrDefault(l => l.Sku == sku)?.Quantity ?? 0;
    }
    
    public IEnumerable<(string Sku, int Quantity, Product Product)> GetExpandedLines()
    {
        foreach (var line in Cart.Lines)
        {
            var product = Catalog.Get(line.Sku);
            yield return (line.Sku, line.Quantity, product);
        }
    }

    public IEnumerable<(string Sku, Product Product)> GetAllUnits()
    {
        foreach (var line in Cart.Lines)
        {
            var product = Catalog.Get(line.Sku);
            for (int i = 0; i < line.Quantity; i++)
                yield return (line.Sku, product);
        }
    }
    
}