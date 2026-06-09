using System.Globalization;
using pricing_engine.Parsing;
using pricing_engine.Pricing;

if (args.Length < 3)
{
    Console.WriteLine("Usage: PricingEngine <catalogPath> <cartPath> <promotions...>");
    return;
}

var cartPath = args[0];
var promotionsPath = args[1];
var customerPath = args[2];

if (!File.Exists(cartPath) ||
    !File.Exists(customerPath) ||
    !File.Exists(promotionsPath))
{
    Console.WriteLine("One or more input files not found.");
    return;
}

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-AU");
ICatalogParser catalogParser = new CatalogFileParser();
var catalog = catalogParser.Parse("./Data/Catalog.txt");

ICartParser cartParser = new CartFileParser();
ICustomerParser customerParser = new CustomerParser();
IPromotionParser promotionParser = new PromotionParser();

var cart = cartParser.Parse(cartPath);
var promotions = promotionParser.Parse(promotionsPath);
var engine = new PricingEngine(catalog, promotions);
var receipt = engine.Checkout(cart, customerParser.Parse(customerPath));

Console.Write(receipt.Render());