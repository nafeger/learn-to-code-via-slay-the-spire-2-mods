# 05 — Beyond the Instance: Static and Namespaces

Everything in this lesson so far has been about *instances* — objects built from a class,
each holding its own state. The scoreboard remembers its own counts; the opponent is its
own thing. That is most of object-oriented programming, but it is not all of it.

Two pieces of code you have been typing since Lesson 3 do not fit the instance picture: the
word `static`, and the `namespace` line at the top of every file. This chapter steps back
and explains both. They are two halves of one idea:

> **Not all code belongs to an individual object.** Some belongs to the *type itself*, and
> every type lives inside a named *container*.

Here is the whole map, which the rest of the chapter fills in:

| Level | Belongs to | You have already typed |
|-------|------------|------------------------|
| **Instance member** | one object | `scoreboard` and its fields (this lesson) |
| **Static member** | the type — one, shared by all | `Rng.Chaotic`, `JacksModMain.ModId` |
| **Namespace** | groups the types | `namespace JacksMod;`, `MegaCrit.Sts2.Core.*` |

Part 1 is the middle row. Part 2 is the bottom row.

---

## Part 1 — Static: belonging to the type, not the instance

### Instance members, recapped

A `Scoreboard` object has its own `playerWins`. Make two scoreboards and you have two
independent counts. That is an **instance member** — there is one copy *per object*, and you
reach it through the object: `scoreboard.Record(...)`. Everything you built this lesson works
this way.

### The bridge you already crossed: `const`

Look back at `Scoreboard`:

```csharp
public class Scoreboard
{
    private const int WINS_NEEDED = 2;   // one value for the whole type
    private int playerWins;              // one copy per object
}
```

`WINS_NEEDED` is different from `playerWins`. It is not "two for this scoreboard, three for
that one" — it is `2`, once, for *every* scoreboard that will ever exist. It belongs to the
**type**, not to any object. A `const` is the first type-level thing you wrote, and it is
the gentle version of the idea this part is about.

### `static`: making a member belong to the type

The `static` keyword says exactly that: "this member belongs to the type, not to any
instance. There is one, shared." A `static` field is a single value shared across the whole
program; a `static` method is a function you call without needing an object.

The tell is in how you *call* it. A static member is reached through the **type name**, with
no object and no `new`:

```csharp
Rng.Chaotic.NextBool();          // through the type Rng — no object created
JacksModMain.ModId;              // through the type JacksModMain
Registry.RegisterEvent<RockPaperScissors>();   // through the type Registry
```

Compare that to the instance calls you have been writing:

```csharp
opponent.Pick();                 // through an object: the opponent
scoreboard.Record(outcome);      // through an object: the scoreboard
```

`opponent.Pick()` needs an `opponent` object first. `Rng.Chaotic.NextBool()` does not need
any object — you call it straight through the type. **That** is the difference, and it is why
your `RpsOpponent` could call `Rng.Chaotic.NextBool()` without ever creating an `Rng`: it is
static. You have been using static members all along; now you know the name for "reached
through a type, no `new`."

### `static class`: a container you never instantiate

Some classes exist *only* to hold static members. You never make an object from them — there
is nothing per-object to make. C# lets you mark the whole class `static` to say so: a
`static class` cannot be instantiated with `new`.

The mod has several, and you have edited them:

```csharp
public static class JacksModMain   // the mod's entry point
{
    public const string ModId = "JacksMod";
    public static void Initialize() { /* ... */ }
}

public static class StarterContent
{
    public static void RegisterStarterContent()
    {
        Registry.RegisterEvent<RockPaperScissors>();
        // ...
    }
}
```

You call `StarterContent.RegisterStarterContent()` through the type — no `new StarterContent()`
anywhere — because the class is static. Registration is a one-time action, not a thing with a
lifetime and per-object state, so a static class is the right home for it. The `Scratch`
class you wrote in Lesson 3's exercise (`public static class Scratch`) was the same: a plain
container for a helper method.

### When *not* to reach for static

Static is shared global state, and that is a double-edged tool. A static field is *one* value
for the entire program — which is exactly wrong for anything you might need more than one of,
or that changes per-thing.

Your `Scoreboard` is deliberately an **instance**, not static. If `playerWins` were a static
field, there would be exactly one score for the whole game, forever — a second match could
not have its own score, because there is only one. Keeping the scoreboard an instance is what
lets each match own its own tally. That was a design choice, and now you can name why it was
right.

The rule of thumb: reach for static for **constants and stateless helpers** (`WINS_NEEDED`,
a registration method, a math utility). Keep **real, changing state on instances**, so you can
have as many independent copies as the program needs. Overusing static for mutable data
recreates the global-variable problems that objects were invented to solve.

---

## Part 2 — Namespaces: organizing the types themselves

Static organizes *members within a type*. A **namespace** organizes the *types within a
program*. It is the same instinct — give things a container — applied one level up.

### What that `namespace` line means

Every file you have written opens with:

```csharp
namespace JacksMod;
```

That line says: "the class in this file belongs to the `JacksMod` group." A namespace is a
named container for types. `Scoreboard`, `RpsOpponent`, `RockPaperScissors` — all of them
live in `JacksMod`, which is why they can see each other by their short names with no extra
ceremony.

### Why namespaces exist: collisions and grouping

A real program pulls in thousands of types. Two libraries might both define a `Random`, or a
`Timer`, or an `Rng`. Without containers their names would collide. Namespaces keep them
apart: the game's random source is `MegaCrit.Sts2.Core.Random.Rng`, which is a *different*
`Rng` from anyone else's, because its full name includes the namespace.

Namespaces also **group related things so you can find them**. The game's code is organized
under `MegaCrit.Sts2.Core.*`:

```
MegaCrit.Sts2.Core.Commands       -> PlayerCmd, CreatureCmd, ...
MegaCrit.Sts2.Core.Events         -> EventOption, ...
MegaCrit.Sts2.Core.Localization   -> L10NLookup, ...
MegaCrit.Sts2.Core.Random         -> Rng
```

The dots are just nesting — `Random` is a namespace inside `Core` inside `Sts2` inside
`MegaCrit`. Related types, grouped by where they live.

### `using` = reaching into another namespace

This finally explains the `using` lines at the top of your files:

```csharp
using MegaCrit.Sts2.Core.Random;   // "let me use types from this namespace by their short name"
```

Without that line, every reference to `Rng` would have to spell out the full
`MegaCrit.Sts2.Core.Random.Rng`. The `using` directive is a per-file convenience: it imports a
namespace so you can write the short name. That is also why **each file declares its own
`using` lines** — a `using` in one file does not carry to another. (That is the rule the
`AdaptiveOpponent` file runs into in Lesson 7: it calls `Rng.Chaotic`, so it needs its own
`using MegaCrit.Sts2.Core.Random;`, even though a sibling file already has one.)

### Closing a loop from chapter 03

In chapter 03 you "moved the `Choice` and `Outcome` enums out to the namespace." Now that
sentence has a precise meaning: the enums went from being nested *inside* `RockPaperScissors`
to living directly in the `JacksMod` namespace. Because every class in the mod is also in
`JacksMod`, they all see `Choice` and `Outcome` by their short names — no `using` needed,
because they share the same namespace. Moving a type "to the namespace" is exactly moving it
up into that shared container.

### File-scoped vs block form

`namespace JacksMod;` with a semicolon is the **file-scoped** form: the whole file belongs to
that namespace. You may see an older **block** form elsewhere, with braces wrapping everything:

```csharp
namespace JacksMod
{
    public class Scoreboard { /* ... */ }
}
```

Same meaning; the file-scoped semicolon form just saves a level of indentation. The mod uses
the file-scoped form throughout.

### A tie-forward to Lesson 7

A type's full identity is its namespace plus its name. Lesson 7 shows the framework deriving
your localization key prefix from the *class name* — `RockPaperScissors` becomes
`ROCK_PAPER_SCISSORS`. That is the same instinct as namespaces: names need containers and
stable identities so they do not collide and so the framework can find them. You will see the
payoff there.

---

## The whole picture

You can now place every name you read:

| If you reach it through… | it is a… | example |
|--------------------------|----------|---------|
| an object (`thing.Member`) | instance member | `scoreboard.Record(...)` |
| a type, no `new` (`Type.Member`) | static member | `Rng.Chaotic`, `Registry.RegisterEvent<...>()` |
| — (it is the container itself) | namespace | `JacksMod`, `MegaCrit.Sts2.Core.Random` |

Most of your *own* code will be instances — objects that hold state, like the scoreboard and
the opponent. Static is for the type-level edges: constants, entry points, registration,
shared utilities. And namespaces are the filing system that keeps every type findable and
distinct. Three levels of belonging, and you have been using all three since before you knew
their names.

## Vocabulary

**Instance member** — A field or method that belongs to an object; one copy per object,
reached through the object.

**Static member** — A field or method that belongs to the type itself; one shared copy,
reached through the type name with no `new`.

**`static class`** — A class that holds only static members and cannot be instantiated. A pure
container, like `JacksModMain` or `StarterContent`.

**Namespace** — A named container that groups types and keeps their names from colliding.
`namespace JacksMod;` declares one; `MegaCrit.Sts2.Core.Random` is one.

**`using` directive** — A per-file import that lets you refer to a namespace's types by their
short names.

**Fully-qualified name** — A type's complete name including its namespace, e.g.
`MegaCrit.Sts2.Core.Random.Rng`.

**Global state** — Data reachable from anywhere, often via static fields. Convenient but hard
to reason about when it changes; the reason the scoreboard is an instance, not static.

## Things to look up

- "C# static keyword" — fields, methods, constructors, and what `static` does to each
- "C# static class" — the rules and when to use one
- "C# namespaces" — declaring, nesting, and the file-scoped form
- "C# using directive" — imports, aliases, and `global using`
- "global variables anti-pattern" — why shared mutable state is risky, across languages
