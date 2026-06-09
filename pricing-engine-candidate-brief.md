# Coding Exercise — Retail Pricing & Promotions Engine

**Stack:** .NET 8 / C#
**Format:** Take-home
**Suggested time:** 3–4 hours. Please don't gold-plate — we'd rather see a smaller scope done well than a large one done loosely.

---

## Scenario

We run a fashion retail store (think Dangerfield) selling apparel and accessories. A customer fills a cart, and at checkout we need to apply the current promotions and produce an **itemised receipt**.

Marketing launches **new promotion types most seasons**. A typical season we'll add one or two kinds of deal we've never run before. We need a checkout that can absorb that without us rewriting and re-testing the parts that already work.

Your job is to build the pricing engine that takes a cart and the active promotions, and returns the receipt.

---

## The catalog

Prices are per unit, in AUD.

| SKU      | Name            | Category    | Unit price |
|----------|-----------------|-------------|-----------:|
| DGFAB360 | Graphic Tee     | Tops        |      49.95 |
| DGFAB412 | Oxford Shirt    | Tops        |      79.95 |
| DGFAB527 | A-Line Dress    | Dresses     |      99.95 |
| DGFAB531 | Wrap Dress      | Dresses     |     119.95 |
| DGFAB648 | Denim Jacket    | Outerwear   |     149.95 |
| DGFAB109 | Patterned Socks | Accessories |      19.95 |
| DGFAB215 | Leather Belt    | Accessories |      39.95 |

A cart is a list of `(SKU, quantity)` lines.

---

## Promotions

These are promotions we'd like to be able to run. **Implement as many of them as you can in the time** — we'd rather see a few modelled cleanly and well tested than all six rushed in. Breadth shows range; depth shows craft. Pick the ones that best let you show how you'd structure this.

1. **Category percentage** — 20% off every item in the *Dresses* category.
2. **Multi-buy** — Buy 2 *Accessories* items, get the cheapest of those 3 free (i.e. "buy 2 get 1 free", applied per group of 3 accessory units in the cart).
3. **Spend threshold** — Spend $200 or more (on the pre-discount subtotal) and take $20 off the order.
4. **Bundle** — A *Tops* item and an *Outerwear* item bought together take $15 off (per matched pair).
5. **Coupon code** — Code `DANGER10` applies 10% off the order. Codes are entered by the customer at checkout; an order may have none.
6. **Member discount** — Signed-in members get an additional 5% off the order.

---

## Input / output contract

Inputs to the engine:
- the **cart** (line items),
- the set of **active promotions**,
- the **customer context** (is-member flag, any coupon code entered).

Output — an **itemised receipt** containing at least:
- each cart line (SKU, name, qty, line price before discounts),
- each **discount applied**, with a human-readable description and the amount (so a customer can see *why* their total changed),
- the **subtotal**, **total discount**, and **final total**.

How you represent these (classes, records, etc.) is up to you. Money must be exact to the cent. Your driver should price at least one representative cart that exercises several of the promotions you implemented.

---

## The rules of the cart — things to think about

You don't need to handle all of these, but they're the kind of question a real checkout has to answer about how a cart behaves, and we'll talk about your choices afterward:

- **Stacking.** Can multiple promotions apply to the same order at once, and in what order are they applied? Is there ever a case where only the *best single* discount should win rather than all of them?
- **Order-wide limits.** Only one coupon code may be used per order.
- **Active windows.** In reality each promotion only runs between a start and end date — pricing the same cart on different dates can give different totals. How would your design handle "is this promotion active right now?" cleanly and testably?
- **The next promotion.** Marketing has already hinted the next one is "spend over $X in a single category." You don't have to build it — but how much of your code would have to change to add it?

---

## What to deliver

- A solution that compiles and runs on .NET 8.
- A command-line entry point that runs the exercise end-to-end and prints a clear, human-readable result.
- **Automated tests.** We care about these — they're part of what we're assessing.
- A short `README` (a few lines): how to build it, how to run it, how to run the tests, and any assumptions or trade-offs you made.

Use any test framework you like (xUnit / NUnit / MSTest). Please don't pull in external libraries for the core logic — we want to see your code.

---

## How we'll assess it

We're looking at how you structure the problem, how you model the domain, the clarity and coverage of your tests, and how easily your design absorbs a new promotion type. Working code that's easy to extend and easy to test beats clever code. Good luck.
