# 02 — Variables

## What is a variable?

A variable is a named box. You put a value in it, give it a name, and use that name to get the value back later.

```csharp
int damage = 8;
//  ------   -
//  name     value
```

You can change the value in the box later:

```csharp
damage = 12;   // now damage holds 12
```

The name doesn't change. The value in the box does.

## Declaring vs assigning

**Declaration** is when you create the variable and tell the compiler what type it holds:

```csharp
int damage;   // declared, but not yet set
```

**Assignment** is when you put a value in it:

```csharp
damage = 8;
```

You usually do both at once:

```csharp
int damage = 8;
```

If you try to use a variable before giving it a value, the compiler will stop you:

```csharp
int damage;
Console.WriteLine(damage);   // error: use of unassigned variable
```

## The two ways to write type declarations

### Explicit type

You spell out the type name yourself:

```csharp
int goldValue = 10;
bool heads = true;
string cardId = "CoinFlip";
```

This is the most readable form. Anyone skimming the code immediately knows what type each variable holds.

### `var` — type inference

You let the compiler figure out the type from the value you give it:

```csharp
var goldValue = 10;    // compiler infers int
var heads = true;      // compiler infers bool
var cardId = "CoinFlip";  // compiler infers string
```

The type is still exactly the same — `goldValue` is still an `int`. You just didn't have to write it. This only works when there's an assignment right there for the compiler to look at.

**When to use which:** Either is valid C#. Explicit types make code easier to read at a glance, especially for beginners. `var` is common when the type is obvious from the right side (`var i = 0` is clear enough) or when the type name is very long. In this curriculum, lean toward explicit types while you're learning.

## Constants

A `const` is a variable that can never change after it's set:

```csharp
const int BASE_GOLD = 10;
const int UPGRADE_BONUS = 2;
```

Convention: constants are usually written in `ALL_CAPS_WITH_UNDERSCORES`.

If you try to change a const:

```csharp
BASE_GOLD = 20;   // error: cannot assign to a constant
```

Use constants when a value has a specific meaning and should never be accidentally changed — like the base damage of a card, or a fixed multiplier.

## Naming variables

Good names make code readable. Bad names make it mysterious.

```csharp
// bad
int x = 10;
int n2 = 5;
bool b = true;

// good
int goldReward = 10;
int upgradeBonus = 5;
bool isHeads = true;
```

C# convention: variable names use `camelCase` — start lowercase, capitalize each new word. (`goldValue`, `cardCost`, `isUpgraded`)

## Things to look up

- "C# variable declaration syntax"
- "var keyword C# explained"
- "C# const vs readonly"

---

Next up: [03-in-the-code.md](03-in-the-code.md) — find these concepts in the mod you already built.
