# Lesson 3: Loops & Recursion

You know how to make decisions (Lesson 2). Now you need to repeat things. Loops are how you do the same operation over a collection of cards, or keep asking the player a question until they give a valid answer.

This lesson also introduces recursion — a method that calls itself. It sounds weird. It turns out to be elegant for certain problems, and you've already seen it in the mod code.

## What you will learn

- `for` loops: when you know how many times to repeat
- `while` loops: when you repeat until a condition is met
- `foreach` loops: when you have a collection and want each item
- Recursion: base case, recursive case, and the call stack
- How to count from 1 to 10 three different ways — and why all three are valid

## Success criteria

You are done with this lesson when you can:

- Write a `for` loop and explain its three parts
- Write a `while` loop and identify its loop condition
- Write a `foreach` loop over a `List<string>`
- Write a simple recursive method with a base case and a recursive case
- Explain what a base case is and why it is required
- Trace a loop manually: write out what the variables are on each iteration
- Explain what "off by one" means

## Files

1. [01-for-loop.md](01-for-loop.md) — Anatomy of a for loop; off-by-one errors; counting backward; summing a list
2. [02-while-and-foreach.md](02-while-and-foreach.md) — While loops; foreach over collections; when to use which
3. [03-recursion.md](03-recursion.md) — What recursion is; base/recursive cases; all three counting methods side by side; stack overflow
4. [04-your-turn.md](04-your-turn.md) — Two exercises: SumCards and tracing DoubleIt

## Connection to the mod

The `TheGoldCoinRoom` event you've already read offers to double your gold repeatedly. The `DoubleIt` method is a kind of loop — it calls `SetEventState` with itself as an option, so the game keeps coming back until you bust or take the gold. That is the event state machine acting as a loop with a break condition.

Lesson 3 gives you the foundation to replace that pattern with explicit, readable loop code — and to build the Rock Paper Scissors event in Lesson 4.

## Up next

[Lesson 4: Methods](../lesson-04-methods/) — You now have every tool you need. Lesson 4 is where you build Rock Paper Scissors.
