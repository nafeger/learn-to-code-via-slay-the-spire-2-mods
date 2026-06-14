# Lesson 7: Interfaces & Inheritance

Every class you have written this curriculum has secretly been part of a bigger structure.
`RockPaperScissors` is `: ModSmithEventModel`. `CoinFlip` is `: ModSmithCardModel`. That
colon is **inheritance**, and it is the mechanism that lets your code plug into the game.
This lesson is where that stops being a magic incantation you copy and becomes something you
understand.

You will learn the two tools — **interfaces** (a contract a class promises to fulfill) and
**inheritance** (a class built on top of another) — by using them on your own opponent: an
easy random one and a hard adaptive one, swappable in a single line. Then you turn the same
lens on the framework and finally see *why* it is shaped the way it is — including the
localization-key rule that has been waiting since Lesson 4.

## What you will learn

- Interfaces: defining a contract, and coding against it instead of a concrete class
- Inheritance: building a class on top of another with `: BaseClass`
- `virtual` and `override`: how a subclass changes inherited behavior
- Polymorphism: one variable, many possible concrete types
- Why `ModSmithEventModel` exists and what it does for you
- **Inversion of control**: why the framework calls *your* code, not the other way around
- The localization key rule: why `RockPaperScissors` needs `ROCK_PAPER_SCISSORS` keys

## Success criteria

You are done with this lesson when you can:

- [ ] Define an interface and write two classes that implement it
- [ ] Explain the difference between an interface and inheritance, and when you would reach
      for each
- [ ] Swap the opponent's strategy in one line and explain why the event needs no other
      change
- [ ] Explain what your event gets from `ModSmithEventModel` and what `override` means
- [ ] Explain, to someone else, exactly why `RPS.options.ROCK` failed and
      `ROCK_PAPER_SCISSORS.options.ROCK` works

## Files

1. [01-interfaces-and-inheritance.md](01-interfaces-and-inheritance.md) — The two tools
2. [02-swappable-opponents.md](02-swappable-opponents.md) — `IOpponent`, easy and hard
3. [03-why-the-framework-works.md](03-why-the-framework-works.md) — Base classes, `override`, inversion of control
4. [04-the-localization-convention.md](04-the-localization-convention.md) — The key rule, explained at last
5. [05-your-turn-and-whats-next.md](05-your-turn-and-whats-next.md) — Exercises and where to go next

## Connection to the mod

This is the lesson where the curriculum's title pays off. "Learn to code via Slay the Spire
2 mods" works because the mod framework is built entirely on the ideas in this lesson. Once
you see that a custom card, a custom relic, and a custom enemy are all *the same move* —
inherit a base class, override its hooks, register, localize — the whole modding surface
opens up. The companion reference, [`MODDING.md`](../../MODDING.md), is the map; this lesson
is the understanding that makes the map readable.

## Up next

[Lesson 8: Debugging](../lesson-08-debugging/) — Break things on purpose and learn to fix
them with real tools.
