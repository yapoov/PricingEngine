

# Promotional Pricing Engine

To prevent margin erosion from unintended discount stacking, promotions are evaluated in a strict sequence from **most specific (item-level)** to **most general (order-level)**.

### Sequence of Execution Matrix

| Step | Promotion Type | Rule Behavior | Applied To |
| --- | --- | --- | --- |
| **1** | **Category Percentage** | 20% off every item in the `Dresses` category. | Individual Item Price |
| **2** | **Multi-Buy** | Buy 2 `Accessories`, get the cheapest of those 3 free. | Item Groups (Sorted Descending) |
| **3** | **Bundle** | A `Tops` item and an `Outerwear` item bought together take $15 off per matched pair. | Pair Groups |
| **4** | **Spend Threshold** | Spend $200 or more to get $20 off the order. | **Pre-discount Subtotal** (Trigger)<br> <br>**Current Subtotal** (Application) |
| **5** | **Coupon Code** | Code `DANGER10` applies 10% off the order. | Progressive Running Total |
| **6** | **Member Discount** | Signed-in members get an additional 5% off the order. | Final Running Total |


## Getting Started

### Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)


### Build the Solution

```bash
dotnet build

```

### Run the CLI


```bash
dotnet run <cart-path> <applied-promotions-path> <customer-context-path>

```

### Run the Automated Tests

```bash
# Nuke local intermediate caches if switching project structures
dotnet clean

# Execute the test runner
dotnet test

