# 03 — Types in the Mod Code

Open `src/StarterContent/cards/CoinFlip.cs` in VS Code. This file is inside the `ModTemplate` folder of your `Sts2-ModSmith` clone — the same VS Code window you had open at the end of Lesson 0.

You're going to read this file looking specifically for types and variables. Don't worry about understanding every line yet — focus only on what you recognize from the last two files.

## Read it first

Read the whole file top to bottom before answering anything. That's a habit worth building.

## Questions to work through

Take a few minutes with your parent before reading the answers below.

**1. Find the `bool` variable**

There's one local variable in `OnPlay` that's a `bool`. What's its name? How is it assigned?

**2. Find the `var` variable**

There's one variable declared with `var`. What's its name? What type do you think the compiler infers for it — and why?

**3. Find a `string` being constructed**

Look for `new LocString(...)`. The arguments inside the parentheses include a string literal. What is it?

**4. Find the `int` comparison**

On the line that reads `base.IsUpgraded ? goldValue / 2 : goldValue` — what's happening here? What type does `goldValue` need to be for division to make sense?

**5. Find the `int` in the constructor**

The constructor at the top reads `base(1, CardType.Skill, ...)`. What does that `1` represent, and what type is it?

---

## The answers

**1.** `bool heads = RunState?.Rng.Niche.NextBool() ?? Rng.Chaotic.NextBool();` — the variable is `heads`, it's assigned the result of a coin flip from the game's random number generator.

**2.** `var goldValue = DynamicVars.Gold.IntValue;` — the compiler infers `int` because `IntValue` is a property that returns an `int`. You'd know this by hovering over it in VS Code (try it — the tooltip shows the type).

**3.** `new LocString("cards", $"{base.Id.Entry}.headsBanter")` — `"cards"` is a plain string literal. The second argument uses a **string interpolation** expression (the `$` prefix and `{}` inside). You'll learn more about that in a later lesson.

**4.** `goldValue / 2` divides the gold amount in half for the penalty on tails. This only makes sense if `goldValue` is a numeric type — and it is: `int`.

**5.** `1` is the card's energy cost. The constructor signature expects an `int` for cost.

## Hover types in VS Code

VS Code's C# extension can show you the type of any expression. Hover over `goldValue` with your mouse. You should see something like:

```
(local variable) int goldValue
```

Try this on `heads`, on `RunState`, on `DynamicVars.Gold`. You're starting to build a habit that will help you read unfamiliar code throughout your career.

## Notice the `?` and `??`

You might have noticed `RunState?.Rng` and `?? Rng.Chaotic`. These are **null-safety operators** — they handle the case where `RunState` doesn't exist outside of combat. You don't need to understand them fully yet, but file the question away: "what does `?` mean after a type name?"

## Things to look up

- "C# string interpolation"
- "C# null conditional operator ?."
- "C# null coalescing operator ??"

---

Next up: [04-your-turn.md](04-your-turn.md) — make a change that runs in the game.
