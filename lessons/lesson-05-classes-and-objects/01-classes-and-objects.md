# 01 — Classes & Objects

## The one-sentence version

A **class** is a blueprint. An **object** is a thing built from that blueprint.

A blueprint for a house is not a house. You cannot live in it. But from one blueprint you
can build many houses, each one a real, separate thing with its own address, its own
furniture, its own state. The blueprint defines what every house *will* have; each house
*is* one.

In code:

- The class says "a scoreboard has a player-win count, a computer-win count, and a draw
  count, and you can record a result on it."
- An object is one actual scoreboard, sitting in memory, with `playerWins = 1` right now.

## You have already been using both

This is not new, it just has not been named. Look at `CoinFlip`:

```csharp
class CoinFlip : ModSmithCardModel
{
    // ...
}
```

That is a **class** — a blueprint for a card. It does not do anything on its own. It
describes what a CoinFlip card is.

When you play a run and a CoinFlip shows up in your deck, the game **creates an object**
from that blueprint. If you have two CoinFlips, there are two objects. They share the
blueprint, but they are separate things — upgrade one and the other is untouched, because
each object holds its own state.

```
   class CoinFlip          <- the blueprint (one, written once)
        │
        ├─ object: a CoinFlip in your hand        <- an instance
        └─ object: another CoinFlip in your deck  <- a different instance
```

"Create an object from a class" has a keyword: `new`.

```csharp
Scoreboard board = new Scoreboard();
```

The right-hand side, `new Scoreboard()`, builds a fresh object. The left-hand side stores
a reference to it. After this line, `board` is one real scoreboard you can use.

## State and behavior, bundled

The reason classes exist — the thing that makes them powerful — is that they bundle two
things that belong together:

- **State**: the data an object remembers. For a scoreboard, that is the three counts.
- **Behavior**: the methods that act on that data. For a scoreboard, that is "record a
  win," "is the match over," "describe the score."

Before now, you have written methods that take all their data as parameters and return a
result — `DetermineOutcome(player, computer)`. That method remembers nothing. Call it a
hundred times and nothing about it changes. That is the right design for pure logic.

But a scoreboard is different. It has to *remember*. The whole point is that recording a
win changes something that persists and affects the next call. State that lives on across
calls is what an object gives you that a lone method cannot.

## Why this matters for the match

Right now, Rock Paper Scissors plays one throw at a time. To make it best-of-three, the
game has to remember the running score *between* the player's choices. A local variable
cannot do that — local variables vanish the moment their method returns (you learned this
as **scope** in Lesson 4).

So the score has to live somewhere that survives across rounds. That "somewhere" is a
**field on an object**. The event will *have a* scoreboard object, and that object will
remember the score for as long as the event is open.

That is the move this lesson teaches: when you need something to persist and to bundle
data with the operations on it, you reach for a class.

## A note on the two skills

Lesson 4 said programming has two separate hard parts: figuring out the problem, and
figuring out how to model it in code. Classes are the main tool for the second part.
"What objects exist in this system, what does each one know, and what can each one do?" is
the central design question of object-oriented programming. You will ask it constantly.

For Rock Paper Scissors, the answer is going to be:

- A **Scoreboard** object knows the running score and can record results.
- An **Opponent** object knows how to pick a throw (and later, will remember your throws).
- The **event** orchestrates them — it *has a* scoreboard and *has an* opponent.

"Has a" is the key phrase. It is the most common relationship between objects, and you
will see it everywhere.

## Vocabulary

**Class** — A blueprint that defines what state and behavior its objects will have.

**Object (instance)** — One concrete thing built from a class with `new`.

**State** — The data an object holds (stored in fields).

**Behavior** — What an object can do (its methods).

**Encapsulation** — Bundling state and the behavior that acts on it into one unit, so the
data and the operations on it live together.

**"Has a"** — The relationship where one object holds another as a field. The event *has a*
scoreboard.

## Things to look up

- "C# class vs object" — any introduction will reinforce the blueprint/instance idea
- "C# new keyword" — how objects are created
- "object-oriented programming" — the broader idea this lesson is one piece of
