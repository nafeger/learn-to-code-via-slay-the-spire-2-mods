# 03 — Switch

## The switch statement

When you have a single variable and a list of specific values to match against, a `switch` is often cleaner than a chain of `else if` comparisons.

```csharp
// With if/else if
if (rarity == "Common")       { cost = 1; }
else if (rarity == "Uncommon") { cost = 2; }
else if (rarity == "Rare")     { cost = 3; }
else                           { cost = 0; }

// Same logic with switch
switch (rarity)
{
    case "Common":
        cost = 1;
        break;
    case "Uncommon":
        cost = 2;
        break;
    case "Rare":
        cost = 3;
        break;
    default:
        cost = 0;
        break;
}
```

Each `case` is a value to match. When a match is found, C# runs that block and then `break` exits the switch. The `default` case is the fallback — like `else`, it runs if nothing matched.

The `break` is required. If you forget it, the compiler will tell you. (In older C, forgetting `break` caused execution to "fall through" into the next case, which was a common source of bugs. C# doesn't allow accidental fallthrough.)

## The switch expression (modern C#)

C# 8 introduced a more concise form called the **switch expression**. Instead of statements inside each case, each arm is an expression that produces a value. There are no `case` or `break` keywords — just the match pattern and the result.

```csharp
int cost = rarity switch
{
    "Common"   => 1,
    "Uncommon" => 2,
    "Rare"     => 3,
    _          => 0,   // _ is the discard pattern — matches anything
};
```

The `_` at the end is the default arm. The whole expression evaluates to one of the values on the right side of `=>`, and that value gets assigned to `cost`.

Switch expressions are preferred in modern C# when you're producing a value from a match. Switch statements are still fine when you need to run multi-line code inside each branch.

## Pattern matching

Switch expressions can match on more than just simple equality. You can match on types, tuples (pairs of values), and conditions.

### Matching a tuple

A tuple is two (or more) values bundled together as one thing. You write it with parentheses: `(value1, value2)`.

Here's a preview of the Rock Paper Scissors logic you'll write in Lesson 4:

```csharp
// preview — you'll build the full version in Lesson 4
string result = (playerChoice, computerChoice) switch
{
    ("Rock",     "Scissors") => "Win",
    ("Paper",    "Rock")     => "Win",
    ("Scissors", "Paper")    => "Win",
    var (p, c) when p == c   => "Draw",
    _                        => "Lose",
};
```

Read this as: "given the pair (playerChoice, computerChoice), match against these patterns."

- The first three arms match specific pairs — Rock beats Scissors, Paper beats Rock, Scissors beats Paper.
- `var (p, c) when p == c` captures both values into `p` and `c`, then applies an extra condition (`when p == c`) to check for a draw.
- `_` catches everything else — the computer wins.

This is more expressive than a chain of `if/else if` comparisons and much harder to get wrong, because every combination is accounted for. If you miss a case, the compiler can warn you.

## When to use switch vs if/else

Use `switch` (or a switch expression) when:
- You're comparing one variable against a list of specific values
- You're producing a single value from a set of known cases
- You're matching combinations (tuples) and the logic is table-like

Use `if`/`else if` when:
- The conditions involve different variables
- The conditions are ranges (`>`, `<`, `>=`) rather than exact values
- You only have two or three cases

Neither is universally better. The goal is code that reads clearly. If you look at it a week later and it makes immediate sense, you made the right call.

## Vocabulary

- **switch statement** — matches a value against a list of cases and runs the matching block
- **case** — a specific value to match in a switch statement
- **break** — exits the current switch case; required at the end of each case block
- **default** — the fallback case in a switch statement; runs when no other case matched
- **switch expression** — modern C# syntax; produces a value from a set of match arms
- **match arm** — one `pattern => result` line in a switch expression
- **discard pattern** — `_`; matches anything; used as the default arm in switch expressions
- **tuple** — two or more values grouped together as one unit, written with parentheses
- **when clause** — an extra condition attached to a match arm with the `when` keyword

## Things to look up

- "C# switch expression"
- "C# pattern matching switch"
- "C# tuple switch expression"

---

Next up: [04-your-turn.md](04-your-turn.md) — add a third option to `TheGoldCoinRoom`.
