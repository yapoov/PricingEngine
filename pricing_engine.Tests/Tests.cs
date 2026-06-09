using pricing_engine.Domain;
using pricing_engine.Pricing;
using pricing_engine.Promotions;

namespace pricing_engine.Tests;

public class PricingEngineTests
{
    private readonly Catalog _mockCatalog;

    public PricingEngineTests()
    {
        // Setup a robust catalog representing the store inventory
        _mockCatalog = new Catalog(new[]
        {
            new Product("DRESS-01", "Evening Dress", "Dresses", 100.00m),
            new Product("ACC-01", "Cheap Belt", "Accessories", 10.00m),
            new Product("ACC-02", "Luxury Scarf", "Accessories", 50.00m),
            new Product("TOP-01", "Silk Blouse", "Tops", 40.00m),
            new Product("OUT-01", "Winter Coat", "Outerwear", 110.00m),
        });
    }

    [Fact]
    public void CategoryPercentage_ShouldApplyTwentyPercentOff_OnlyToDresses()
    {
        // Arrange
        var cart = new Cart(new List<CartLine>
        {
            new("DRESS-01", 2), // $200.00 total
            new("TOP-01", 1) // $40.00 total
        });

        var customer = new CustomerContext(false, "");
        var promotions = new List<IPromotion> { new CategoryPercentagePromotion("Dresses", 0.20m) };
        var engine = new PricingEngine(_mockCatalog, promotions);

        // Act
        var receipt = engine.Checkout(cart, customer);

        // Assert: 20% off $200 is $40. The Top item ($40) is untouched.
        Assert.Equal(240.00m, receipt.Subtotal);
        Assert.Equal(40.00m, receipt.TotalDiscount);
        Assert.Equal(200.00m, receipt.Total);
        Assert.Contains(receipt.Discounts, d => d.Description.Contains("20% off Dresses"));
    }

    [Fact]
    public void MultiBuyAccessory_ShouldSortDescending_AndDiscountCheapestInGroupOfThree()
    {
        // Arrange
        // Buying 4 items: Three expensive ($50 x 3), one cheap ($10 x 1)
        var cart = new Cart(new List<CartLine>
        {
            new("ACC-02", 3), // $150.00
            new("ACC-01", 1) // $10.00
        });

        var customer = new CustomerContext(false, "");
        var promotions = new List<IPromotion> { new MultiBuyAccessoryPromotion("Accessories") };
        var engine = new PricingEngine(_mockCatalog, promotions);

        // Act
        var receipt = engine.Checkout(cart, customer);

        // Assert: Grouping sorts [50, 50, 50, 10]. First 3 group together, making the 3rd $50 item FREE. 
        // A naive ascending sort would have given away the $10 item instead.
        Assert.Equal(160.00m, receipt.Subtotal);
        Assert.Equal(50.00m, receipt.TotalDiscount);
        Assert.Equal(110.00m, receipt.Total);
    }

    [Fact]
    public void SpendThreshold_ShouldTriggerOnPreDiscountSubtotal_EvenIfPriorPromosDropCurrentTotalBelowThreshold()
    {
        // Arrange
        var cart = new Cart(new List<CartLine>
        {
            new("DRESS-01", 2), // $200.00 (Triggers the $200 threshold)
            new("TOP-01", 1) // $40.00
        }); // Pre-discount total = $240.00

        var customer = new CustomerContext(false, "");

        // Pipeline: Category percentage strips $40 instantly, dropping running total to $200.
        var promotions = new List<IPromotion>
        {
            new CategoryPercentagePromotion("Dresses", 0.20m), // -$40.00
            new SpendThresholdPromotion(200.00m, 20.00m) // Should still hit!
        };
        var engine = new PricingEngine(_mockCatalog, promotions);

        // Act
        var receipt = engine.Checkout(cart, customer);

        // Assert
        Assert.Equal(240.00m, receipt.Subtotal);
        Assert.Equal(60.00m, receipt.TotalDiscount); // $40 + $20
        Assert.Equal(180.00m, receipt.Total);
    }

    [Fact]
    public void CouponsAndMembership_ShouldCalculatePercentages_OnProgressiveRunningTotals()
    {
        // Arrange
        var cart = new Cart(new List<CartLine>
        {
            new("DRESS-01", 2) // $200.00
        });

        var customer = new CustomerContext(IsMember: true, CouponCode: "DANGER10");

        var promotions = new List<IPromotion>
        {
            new CategoryPercentagePromotion("Dresses", 0.20m), // $200 -> $160 (-$40)
            new CouponPromotion("DANGER10", 0.10m), // 10% of $160 = -$16.00 ($144 remaining)
            new MemberDiscountPromotion(0.05m) // 5% of $144 = -$7.20 ($136.80 remaining)
        };
        var engine = new PricingEngine(_mockCatalog, promotions);

        // Act
        var receipt = engine.Checkout(cart, customer);

        // Assert
        Assert.Equal(200.00m, receipt.Subtotal);
        Assert.Equal(63.20m, receipt.TotalDiscount); // 40.00 + 16.00 + 7.20
        Assert.Equal(136.80m, receipt.Total);
    }

    [Fact]
    public void BundlePromotion_ShouldApplyDiscount_PerMatchedPair()
    {
        // Arrange
        var cart = new Cart(new List<CartLine>
        {
            new("TOP-01", 3), // 3 Tops
            new("OUT-01", 2) // 2 Outerwears
        }); // Total matched pairs possible = 2

        var customer = new CustomerContext(false,"");
        var promotions = new List<IPromotion> { new BundlePromotion("Tops", "Outerwear", 15.00m) };
        var engine = new PricingEngine(_mockCatalog, promotions);

        // Act
        var receipt = engine.Checkout(cart, customer);

        // Assert: 2 pairs x $15.00 = $30.00 discount
        decimal expectedSubtotal = (3 * 40.00m) + (2 * 110.00m); // 120 + 220 = 340
        Assert.Equal(expectedSubtotal, receipt.Subtotal);
        Assert.Equal(30.00m, receipt.TotalDiscount);
        Assert.Equal(310.00m, receipt.Total);
    }
}