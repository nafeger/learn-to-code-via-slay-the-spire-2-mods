# 01 — Types

## What is a type?

A type is a promise about what a value is and what you can do with it.

If I hand you a piece of paper that says `7`, you might ask: seven what? Seven dollars? Seven damage? Seven cards? The number alone isn't enough — you need context to know how to use it.

Types are that context. When the compiler knows a value is an `int`, it knows you can add it to other `int`s, compare it, pass it to methods that expect a number. When it knows a value is a `string`, it knows you can combine it with other text, check its length, look for characters inside it.

The compiler uses this information to catch mistakes before the game ever runs. That's exactly what happened in Lesson 0 — `TalkCmd.Play` expected a `VfxColor` and the template was passing a `double`. The types didn't match, so the compiler refused to build.

## The types you'll use most

### `int`

A whole number — no decimals. Positive or negative, including zero.

```csharp
int damage = 8;
int cost = 1;
int cardsInHand = 5;
```

In the mod: card cost, damage values, gold amounts, block values, power stacks — all `int`.

### `float` and `double`

Numbers with decimal points.

- `float` is 32 bits of precision (roughly 7 significant digits)
- `double` is 64 bits of precision (roughly 15 significant digits)

```csharp
float percentage = 0.5f;   // the 'f' suffix marks it as float
double preciseValue = 0.123456789012345;
```

You'll use these when you need fractions — chance calculations, multipliers, anything that can't be expressed as a whole number. In STS2 modding you'll see `double` come up in probability calculations.

### `string`

Text. Any characters, any length, wrapped in double quotes.

```csharp
string modName = "FirstSpireMod";
string cardId = "CoinFlip";
string empty = "";
```

In the mod: card names, localization keys, log messages — all strings.

### `bool`

A value that's either `true` or `false`. Nothing else.

```csharp
bool isUpgraded = false;
bool heads = true;
bool playerHasGold = false;
```

In the mod: `IsUpgraded` on a card, the result of a coin flip, whether a creature is on a given side.

## Types are enforced by the compiler

In C#, once you declare a variable with a type, that's what it holds. You can't put a `string` into an `int` variable:

```csharp
int cost = "expensive";   // error: cannot convert string to int
```

This feels restrictive at first. It's actually a feature — the compiler catches a whole class of bugs before they become runtime crashes.

## There's a lot more

There are many other types: `char` (a single character), `decimal` (exact fixed-point arithmetic, used for money), collections like `List<int>`, your own custom types like `CoinFlip` or `GoldArmor`. You'll meet them as you need them. For now these four will take you a long way.

## Things to look up

- "C# value types vs reference types"
- "what is a primitive type in programming"
- "C# int vs float vs double"

---

Next up: [02-variables.md](02-variables.md) — declaring and naming variables.
