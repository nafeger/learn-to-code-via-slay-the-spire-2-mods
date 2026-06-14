# Verified RPS logic (Lessons 5–7)

This is a small, runnable console app that holds the **framework-independent** Rock Paper
Scissors logic from Lessons 5, 6, and 7. It exists so the code in those lessons is not just
listings on a page — it actually compiles and runs, and a demo proves it behaves the way the
lessons say.

## What's here

The opponent and scoreboard classes are **the same code the lessons tell you to write**:

| File | Built in | What it is |
|------|----------|------------|
| `RpsTypes.cs` | Lesson 5 | The `Choice` and `Outcome` enums |
| `Scoreboard.cs` | Lesson 5 | Best-of-three score tracking |
| `IOpponent.cs` | Lesson 7 | The opponent contract |
| `RandomOpponent.cs` | Lesson 7 | Easy opponent (picks at random) |
| `AdaptiveOpponent.cs` | Lessons 6–7 | Hard opponent (counters your recent favorite) |

Two files are **not** from the lessons — they only exist so the code can run outside the
game:

- `GameStubs.cs` — a tiny stand-in for the game's `Rng.Chaotic` random source. In the real
  mod, Slay the Spire 2 provides this; here a seeded `System.Random` fills in.
- `Program.cs` — a demo harness that drives the classes and checks their behavior.

## The boundary

The `RockPaperScissors` **event** itself is not here. It inherits from `ModSmithEventModel`
and uses `EventOption`, `PlayerCmd`, `L10NLookup`, and other framework types that only exist
inside Slay the Spire 2 — so it cannot compile standalone. That code lives in the lessons as
listings, and runs in the real mod project. What you *can* verify on its own is the logic the
lessons make claims about: the scoreboard, and whether the adaptive opponent really adapts.
That is exactly what this project checks.

## Run it

From the repository root:

```
dotnet run --project examples/rps-logic
```

Expected output: a best-of-three match plays to a single winner, the adaptive opponent throws
the counter to a predictable player well over half the time, and the random opponent does not
adapt. The program exits `0` when every check passes.
