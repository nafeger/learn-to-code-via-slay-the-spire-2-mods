# Lesson 4: Methods

You have types, control flow, and loops. There is one more fundamental building block before you can write real programs: methods.

A method is a named, reusable block of code. Instead of writing the same logic twice, you give it a name, put it in one place, and call it wherever you need it. Methods also let you break a big problem into small, testable pieces. That is the central skill this lesson builds.

The capstone project is a working Rock Paper Scissors event inside Slay the Spire 2. You will design it, trace through it by hand, write it, register it, and run it.

## What you will learn

- What a method is and why they exist
- Anatomy of a method: access modifier, return type, name, parameters, body
- The difference between `void` and a method that returns a value
- How to call a method and use its return value
- What scope means and why local variables stay inside their method
- The difference between `private` and `public`
- How to break a problem into methods before writing any code

## Success criteria

You are done with this lesson when you can:

- [ ] Define a method with parameters and a return type, then call it
- [ ] Explain what `void` means and give an example of when to use it
- [ ] Explain the difference between `private` and `public` and name a reason to prefer `private`
- [ ] Look at `DetermineOutcome` and explain what its parameters are and what it returns
- [ ] Look at `PickComputerChoice` and trace through its logic by hand, listing all possible return values
- [ ] Build the Rock Paper Scissors event and encounter it in game

## Files

1. [01-methods.md](01-methods.md) — What a method is; anatomy; void vs return; calling methods; examples from the mod
2. [02-scope-and-access.md](02-scope-and-access.md) — Local variables and scope; private vs public; why private is the right default
3. [03-building-rps.md](03-building-rps.md) — Designing the event method by method before writing a single line
4. [04-the-event.md](04-the-event.md) — The full implementation, registration, and running it in game
5. [05-localization.md](05-localization.md) — Optional: what localization files are and how the text system works

## Connection to the mod

The `TheGoldCoinRoom` event you have been studying has two helper methods: `TakeGold` and `DoubleIt`. Each one does exactly one thing. That separation is not accidental — it makes each part easy to read and easy to change independently.

Rock Paper Scissors takes that pattern further. The game logic (`DetermineOutcome`) is completely separate from the random selection (`PickComputerChoice`), which is completely separate from the event wiring (`Play`). You could test each piece in isolation without touching the others.

That is what well-organized methods look like.

## Up next

[Lesson 5: Classes and Objects](../lesson-05-classes-and-objects/) — Methods live inside classes. Lesson 5 is where you learn to design your own classes and understand what `this`, constructors, and fields really mean.
