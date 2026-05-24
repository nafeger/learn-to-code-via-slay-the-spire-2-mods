# 01 — Conditions

## What is a condition?

A condition is an expression that evaluates to either `true` or `false`. That's it. Every `if` statement you write is asking a yes/no question, and the condition is the question.

```csharp
bool heads = true;

// "Is heads true?" — yes, so this runs
if (heads)
{
    // ...
}
```

Because `bool` values are already true or false, you can pass them directly as a condition. But most of the time you'll build a condition by comparing two values.

## Comparison operators

These operators compare two values and produce a `bool` result.

| Operator | Meaning | Example | Result |
|---|---|---|---|
| `==` | equal to | `gold == 0` | `true` if gold is zero |
| `!=` | not equal to | `gold != 0` | `true` if gold is nonzero |
| `<` | less than | `damage < 5` | `true` if damage is under 5 |
| `>` | greater than | `damage > 5` | `true` if damage is over 5 |
| `<=` | less than or equal | `cost <= 1` | `true` if cost is 1 or less |
| `>=` | greater than or equal | `gold >= 30` | `true` if gold is 30 or more |

```csharp
int gold = 25;

Console.WriteLine(gold >= 30);  // false
Console.WriteLine(gold > 0);    // true
Console.WriteLine(gold == 25);  // true
Console.WriteLine(gold != 25);  // false
```

## Logical operators

When one condition isn't enough, you combine them with logical operators.

### `&&` — AND

Both sides must be true for the whole expression to be true.

```csharp
int gold = 50;
bool isUpgraded = true;

bool spendGold = gold > 0 && isUpgraded;  // true — both are true
```

If the left side is false, C# skips the right side entirely. There's no way the result can be true if the first condition failed.

### `||` — OR

Either side being true makes the whole expression true.

```csharp
bool freeCard = isUpgraded || gold > 100;  // true if either condition holds
```

If the left side is true, C# skips the right side. The result is already determined.

### `!` — NOT

Flips a boolean: `true` becomes `false`, `false` becomes `true`.

```csharp
bool isUpgraded = false;
bool notUpgraded = !isUpgraded;  // true
```

## Short-circuit evaluation

The "skipping" behavior described above is called **short-circuit evaluation**. It matters for two reasons.

**Performance:** if the left side of `&&` is expensive to compute and likely to be false, put it first. The right side only runs when necessary.

**Safety:** short-circuiting prevents errors. Consider:

```csharp
// dealer might be null — calling dealer.Player on a null value would crash
if (dealer != null && dealer.Player != null)
{
    // safe — we only reach dealer.Player if dealer isn't null
}
```

The right side (`dealer.Player != null`) is only evaluated if the left side (`dealer != null`) is true. If `dealer` is null and C# evaluated both sides regardless, the program would crash.

The `?.` operator (null-conditional) you saw in `CoinFlip.cs` is related to this — it's another way to safely access members that might be null. You'll see more of that in later lessons.

## A real example: MadeOfGold.cs

Open `src/StarterContent/powers/MadeOfGold.cs`. Find this line:

```csharp
if (target == Owner && dealer?.Player != null && result.UnblockedDamage > 0 && props.HasFlag(ValueProp.Move) && !props.HasFlag(ValueProp.Unpowered))
```

This is five conditions joined by `&&`. All five must be true for the player to gain gold. Read them left to right:

1. `target == Owner` — the thing that got hit is the creature this power is attached to (not some other creature)
2. `dealer?.Player != null` — the attacker is a player, not an enemy (the `?.` safely handles the case where `dealer` is null)
3. `result.UnblockedDamage > 0` — at least some damage got through the block (didn't just bounce off armor)
4. `props.HasFlag(ValueProp.Move)` — the damage came from an attack card (a "move"), not poison or another source
5. `!props.HasFlag(ValueProp.Unpowered)` — the attack isn't marked as unpowered (a flag that disables certain effects)

Each condition is simple on its own. The power of `&&` is that you can layer multiple simple checks into one precise rule.

Because they're all connected by `&&`, if condition 1 is false (wrong target), C# skips the remaining four checks and moves on. Short-circuiting in action.

## Vocabulary

- **condition** — an expression that evaluates to `true` or `false`
- **boolean expression** — another name for a condition; any expression of type `bool`
- **comparison operator** — `==`, `!=`, `<`, `>`, `<=`, `>=`; compares two values and returns a bool
- **logical operator** — `&&`, `||`, `!`; combines or inverts boolean values
- **short-circuit evaluation** — stopping evaluation of a logical expression as soon as the result is determined
- **null** — the absence of a value; accessing members on a null reference causes a runtime crash

## Things to look up

- "C# comparison operators"
- "C# short-circuit evaluation && ||"
- "C# null conditional operator ?."

---

Next up: [02-if-else.md](02-if-else.md) — putting conditions to work with `if`, `else`, and `else if`.
