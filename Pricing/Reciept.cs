using System.Globalization;

namespace pricing_engine.Pricing;

public class Receipt
{
    public List<ReceiptLine> Lines { get; } = new();
    public List<DiscountLine> Discounts { get; } = new();

    public decimal Subtotal { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal Total => Subtotal - TotalDiscount;

    public string Render()
    {
        var output = new System.Text.StringBuilder();

        output.AppendLine("ITEMS:");
        foreach (var l in Lines)
            output.AppendLine($"{l.Sku} {l.Name} x{l.Quantity} = {l.LineTotal:C}");

        output.AppendLine("\nDISCOUNTS:");
        foreach (var d in Discounts)
            output.AppendLine($"{d.Description}: -{d.Amount:C}");

        output.AppendLine($"\nSUBTOTAL: {Subtotal:C}");
        output.AppendLine($"DISCOUNT: {TotalDiscount:C}");
        output.AppendLine($"TOTAL: {Total:C}");

        return output.ToString();
    }
}

public record ReceiptLine(string Sku, string Name, int Quantity, decimal LineTotal);

public record DiscountLine(string Description, decimal Amount);