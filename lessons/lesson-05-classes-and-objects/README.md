# Lesson 5: Classes & Objects

You have been writing methods inside a class this whole time — `RockPaperScissors` is a
class. But you have treated it as a container, not as a thing you design. This lesson is
where that changes.

A **class** is a blueprint. An **object** is a thing built from that blueprint. A class
bundles together *state* (the data it remembers) and *behavior* (the methods that act on
that data) into one unit you can name, create, and reason about.

The payoff: Rock Paper Scissors becomes a **best-of-three match** with a running
**scoreboard**. To make that work, the game needs to *remember* the score across rounds —
and "an object that remembers things" is exactly what a class is for.

Once you have built objects, the lesson zooms out to the two things that are *not* per-object:
the `static` keyword (code that belongs to the type, not any instance) and `namespace` (the
named container all your types live in). You have been typing both since Lesson 3 without an
explanation; here is where they get one.

## What you will learn

- The difference between a class (the blueprint) and an object (an instance)
- Fields: the data an object remembers
- Methods as behavior that acts on an object's own fields
- Constructors: how an object is set up when it is created
- What `this` refers to
- Why bundling state and behavior together (encapsulation) is the whole point
- When a type should move *out* of one class so others can share it
- The difference between an instance member and a `static` member (one shared by the type)
- What a `static class` is and why `JacksModMain` and `StarterContent` are static
- What a `namespace` is, why every file declares one, and what `using` actually does

## Success criteria

You are done with this lesson when you can:

- [ ] Explain the difference between a class and an object in your own words
- [ ] Write a class with private fields and public methods that act on them
- [ ] Explain why the scoreboard has to be a *field* on the event, not a local variable
- [ ] Build the `Scoreboard` class and the `RpsOpponent` class
- [ ] Make Rock Paper Scissors a best-of-three match and see the score persist across rounds in game
- [ ] Tell an instance member from a static member by how it is called, and say why the scoreboard is an instance and not static
- [ ] Explain what `namespace JacksMod;` and a `using` directive do

## Files

1. [01-classes-and-objects.md](01-classes-and-objects.md) — Blueprint vs instance; what a card really is
2. [02-fields-constructors-this.md](02-fields-constructors-this.md) — State, setup, and `this`
3. [03-building-the-scoreboard.md](03-building-the-scoreboard.md) — Your first class from scratch
4. [04-the-opponent-as-an-object.md](04-the-opponent-as-an-object.md) — Extract the opponent; wire up best-of-three
5. [05-static-and-namespaces.md](05-static-and-namespaces.md) — The two things that are *not* per-object: `static` and namespaces
6. [06-your-turn.md](06-your-turn.md) — Exercises

## Connection to the mod

Look at `CoinFlip`. It is a class: `class CoinFlip : ModSmithCardModel`. When the game
deals you a CoinFlip during a run, it *creates an object* from that class. If two
CoinFlips are in your deck, there are two objects — same blueprint, two independent
things. That is the class/object distinction, and it is sitting in code you already have.

This lesson takes the same idea and turns it on your own design: the opponent and the
scoreboard become classes, and the running match becomes a set of cooperating objects.

## Up next

[Lesson 6: Collections](../lesson-06-collections/) — The opponent gets a *memory*: a list
of your recent throws, which it uses to start predicting you.
