# Learn to Code via Slay the Spire 2 Mods

A hands-on programming curriculum built around modding [Slay the Spire 2](https://store.steampowered.com/app/1916400/Slay_the_Spire_2/). Each lesson teaches a core programming concept and ends with something visible running inside the game.

The curriculum is designed for someone who has seen a little code before (Scratch, maybe some C) but hasn't built a systematic foundation yet. By the end you'll have written a working mod, understand the fundamentals of object-oriented programming, and have the math groundwork needed to go deeper into probability and data modeling.

## Prerequisites

- A copy of Slay the Spire 2 (Early Access, Steam)
- A Mac or Windows machine
- Curiosity

## Curriculum

| Lesson | Topic | Payoff |
|--------|-------|--------|
| [Lesson 0](lessons/lesson-00-setup/) | Environment & Tooling | Your mod loads in the game |
| [Lesson 1](lessons/lesson-01-types-and-variables/) | Types & Variables | Change gold values, see it in game |
| [Lesson 2](lessons/lesson-02-control-flow/) | Control Flow | Weighted coin flip card |
| [Lesson 3](lessons/lesson-03-loops-and-recursion/) | Loops & Recursion | Rock Paper Scissors, single round |
| [Lesson 4](lessons/lesson-04-methods/) | Methods | RPS best-of-3 event room |
| [Lesson 5](lessons/lesson-05-classes-and-objects/) | Classes & Objects | Best-of-three RPS with a scoreboard object |
| [Lesson 6](lessons/lesson-06-collections/) | Collections | An opponent that remembers and counters your throws |
| [Lesson 7](lessons/lesson-07-interfaces-and-inheritance/) | Interfaces & Inheritance | Swappable opponents; why the mod framework works the way it does |
| [Lesson 8](lessons/lesson-08-debugging/) | Debugging | Break things on purpose, fix them with VS Code |
| [Lesson 9](lessons/lesson-09-probability-and-randomness/) | Probability & Randomness | Expected value, the math behind the game's RNG |
| [Lesson 10](lessons/lesson-10-capstone/) | Capstone | Design and ship your own card from scratch |

## Reference

[**MODDING.md**](MODDING.md) — a map of what a Slay the Spire 2 mod can add (cards, relics,
powers, potions, events, and more), the four-step pattern every piece of content follows,
and the conventions that trip up new modders. Lessons 5–7 build toward it; read it once
Lesson 7 makes it click.

[**examples/rps-logic/**](examples/rps-logic/) — the framework-independent code from Lessons
5–7 (the scoreboard and the opponents) as a runnable console app, so it is verified rather
than just printed. `dotnet run --project examples/rps-logic` plays a match and confirms the
adaptive opponent really does counter a predictable player.

## Licenses

The **code** in this repository is licensed under the [MIT License](LICENSE-CODE).

The **lesson content** (text, exercises, explanations) is licensed under [Creative Commons Attribution 4.0 International (CC BY 4.0)](LICENSE-CONTENT). You are free to use, share, and adapt the lessons as long as you give appropriate credit.

## Attribution

Created by [Nate Feger](https://github.com/nafeger).
