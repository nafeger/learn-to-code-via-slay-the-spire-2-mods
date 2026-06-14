# Lesson 6: Collections

A single value holds one thing. A **collection** holds many. Almost every interesting
program is mostly moving groups of things around — a deck of cards, a list of enemies, a
tally of how often something happened — and C# gives you a small set of collection types to
do it well.

This lesson teaches the two you will reach for constantly: the **list** (an ordered
sequence) and the **dictionary** (a lookup from keys to values). Then you use both to give
the Rock Paper Scissors opponent a **memory**: it records your recent throws in a list,
tallies them in a dictionary, and **biases its next pick to counter whatever you throw
most**. Spam Rock and it starts answering with Paper.

## What you will learn

- `List<T>`: adding, counting, indexing, and iterating an ordered sequence
- `Dictionary<TKey, TValue>`: storing and looking up values by key
- The **tally** pattern — counting occurrences with a dictionary
- Keeping a fixed-size "recent window" so memory does not grow forever
- That localization itself is a dictionary — the same idea you have used since Lesson 4
- Turning remembered data into a decision (the bias)

## Success criteria

You are done with this lesson when you can:

- [ ] Create a `List<T>`, add to it, and iterate it with `foreach`
- [ ] Create a `Dictionary<TKey, TValue>`, store values, and read them back by key
- [ ] Explain the tally pattern and write one that counts items in a list
- [ ] Give `RpsOpponent` a memory of recent throws and keep it to a fixed size
- [ ] Make the opponent bias its pick from that memory and feel it adapt in game

## Files

1. [01-lists.md](01-lists.md) — `List<T>`: ordered sequences
2. [02-dictionaries.md](02-dictionaries.md) — `Dictionary<TKey, TValue>` and the tally pattern
3. [03-giving-the-opponent-memory.md](03-giving-the-opponent-memory.md) — A list of recent throws
4. [04-biasing-the-pick.md](04-biasing-the-pick.md) — Tally the memory, counter the favorite
5. [05-your-turn.md](05-your-turn.md) — Exercises

## Connection to the mod

You have already used a collection in every event and card you have written. `CanonicalVars`
returns `IEnumerable<DynamicVar>` — a collection. The `[ ... ]` you write for event options
is a collection literal. And the localization JSON is a dictionary: keys mapped to text.
This lesson names the tools you have been using and puts them to work on your own data.

## Up next

[Lesson 7: Interfaces & Inheritance](../lesson-07-interfaces-and-inheritance/) — Make the
opponent's *strategy* swappable, and finally understand why the whole mod framework is
shaped the way it is.
