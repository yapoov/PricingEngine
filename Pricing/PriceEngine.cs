using pricing_engine.Domain;
using pricing_engine.Promotions;

namespace pricing_engine.Pricing;

public class PricingEngine
{
    private readonly Catalog _catalog;
    private readonly List<IPromotion> _promotions;

    public PricingEngine(Catalog catalog, List<IPromotion> promotions)
    {
        _catalog = catalog;
        _promotions = promotions;
    }

    public Receipt Checkout(Cart cart, CustomerContext customer)
    {
        var context = new PricingContext(_catalog, cart, customer);

        var receipt = new Receipt();

        decimal subtotal = 0;

        foreach (var line in cart.Lines)
        {
            var product = _catalog.Get(line.Sku);
            var lineTotal = product.Price * line.Quantity;

            receipt.Lines.Add(new ReceiptLine(
                line.Sku,
                product.Name,
                line.Quantity,
                lineTotal));

            subtotal += lineTotal;
        }

        receipt.Subtotal = subtotal;

        foreach (var promo in _promotions)
        {
            var discounts = promo.Apply(context);
            
            foreach (var d in discounts)
            {
                receipt.Discounts.Add(new DiscountLine(d.Description, d.Amount));
                context.AddDiscount(d);    
            }
        }

        receipt.TotalDiscount = receipt.Discounts.Sum(d => d.Amount);
        return receipt;
    }
}