# 02 — if / else

## The basic shape

An `if` statement runs a block of code only when a condition is true.

```csharp
if (condition)
{
    // runs when condition is true
}
```

An `else` block runs when the condition is false.

```csharp
if (condition)
{
    // runs when condition is true
}
else
{
    // runs when condition is false
}
```

You've already seen this in `TheGoldCoinRoom.cs`:

```csharp
private Task DoubleIt()
{
    var doubled = Rng.NextBool();
    if (doubled)
    {
        DynamicVars.Gold.BaseValue *= 2;
        SetEventState(...);
    }
    else
    {
        SetEventFinished(...);
    }
    return Task.CompletedTask;
}
```

`Rng.NextBool()` returns a random `bool`. If it's `true`, the gold doubles and the event continues. If it's `false`, the run ends. Exactly one of the two branches runs — never both, never neither.

## else if: three or more paths

When you have more than two possible outcomes, chain `else if` between your `if` and final `else`.

```csharp
int gold = DynamicVars.Gold.IntValue;

if (gold >= 60)
{
    Console.WriteLine("You have a lot of gold.");
}
else if (gold >= 30)
{
    Console.WriteLine("You have a decent amount of gold.");
}
else
{
    Console.WriteLine("You're running low.");
}
```

C# checks each condition from top to bottom and runs the first block whose condition is true. Once a branch runs, the rest are skipped. The final `else` is a fallback that runs only if every condition above it was false.

Order matters. If you put `gold >= 30` before `gold >= 60`, someone with 75 gold would match the first condition and never reach the second:

```csharp
// Wrong order — gold >= 30 catches everything 30 and above,
// so gold >= 60 is unreachable
if (gold >= 30) { ... }
else if (gold >= 60) { ... }  // never runs
```

Always order your conditions from most specific (or most restrictive) to least.

## Nesting

You can put an `if` inside another `if`. This is called nesting.

```csharp
if (gold > 0)
{
    if (isUpgraded)
    {
        // has gold AND is upgraded
    }
    else
    {
        // has gold but NOT upgraded
    }
}
```

Nesting works but it can get hard to read fast. When you find yourself three levels deep, it's usually a sign you could restructure with `&&` or extract some logic into its own method. For now, one or two levels is fine.

## The ternary operator

You saw this in `CoinFlip.cs`:

```csharp
goldValue = base.IsUpgraded ? goldValue / 2 : goldValue;
```

The `? :` operator is a compact `if`/`else` that produces a value. The structure is:

```
condition ? value_if_true : value_if_false
```

So that line reads: "if the card is upgraded, set goldValue to goldValue divided by 2; otherwise, keep goldValue as it is."

The ternary is useful when you're assigning one of two values based on a condition and both outcomes are short. Avoid it when either branch is complex — an `if`/`else` is easier to read.

```csharp
// Good use of ternary — short, clear
string label = isUpgraded ? "Upgraded" : "Base";

// Too much for a ternary — use if/else instead
int result = isUpgraded ? SomeComplexMethod(a, b, c) : AnotherComplexMethod(x, y, z);
```

## A common mistake: = vs ==

One of the most common bugs beginners write is accidentally using assignment (`=`) where they meant comparison (`==`).

```csharp
int gold = 20;

// This is a comparison — asks "is gold equal to 20?"
if (gold == 20) { ... }

// This is assignment — sets gold to 20, which is valid but almost certainly wrong here
// C# will warn you about this, but it's worth knowing why
if (gold = 20) { ... }
```

In C#, the compiler will usually warn you if you put an assignment in a condition because the result is an `int`, not a `bool`. But the habit of reaching for `==` is important to build — in other languages (and with `bool` assignments) the compiler may not catch it.

When you read code and something seems to always be true no matter what, check for `=` where `==` was intended.

## Vocabulary

- **if statement** — runs a block of code only when a condition is true
- **else** — the fallback block that runs when the `if` condition is false
- **else if** — a second (or third, fourth...) condition to check if the first was false
- **branch** — one of the paths through an `if`/`else` structure
- **nesting** — placing one `if` inside another
- **ternary operator** — the `? :` construct; a compact if/else that produces a value
- **assignment** — `=`, sets a variable to a value
- **equality operator** — `==`, compares two values

## Things to look up

- "C# if else syntax"
- "C# ternary operator"
- "C# assignment vs equality operator bug"

---

Next up: [03-switch.md](03-switch.md) — switch statements and the modern switch expression.
