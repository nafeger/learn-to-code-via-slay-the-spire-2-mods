# 02 — Swappable Opponents

Now apply the two tools to the opponent. You will split the tangled `RpsOpponent` into a
contract and two implementations, then swap difficulty in one line.

## The contract

Create `src/StarterContent/events/IOpponent.cs`:

```csharp
namespace JacksMod;

public interface IOpponent
{
    Choice Pick();
    void Remember(Choice playerThrow);
}
```

This is the promise every opponent makes: it can be asked to pick, and to remember. The
event will hold an `IOpponent` and call exactly these two methods — nothing else.

## The easy opponent

`RandomOpponent` is the base. It picks at random and ignores history. Replace
`RpsOpponent.cs` with `RandomOpponent.cs`:

```csharp
using MegaCrit.Sts2.Core.Random;

namespace JacksMod;

public class RandomOpponent : IOpponent
{
    public virtual Choice Pick() => RandomChoice();

    public virtual void Remember(Choice playerThrow)
    {
        // An easy opponent does not care what you have thrown. Nothing to do.
    }

    protected Choice RandomChoice()
    {
        if (Rng.Chaotic.NextBool()) return Choice.Rock;
        if (Rng.Chaotic.NextBool()) return Choice.Paper;
        return Choice.Scissors;
    }

    protected Choice CounterTo(Choice c) => c switch
    {
        Choice.Rock     => Choice.Paper,
        Choice.Paper    => Choice.Scissors,
        Choice.Scissors => Choice.Rock,
        _               => Choice.Rock,
    };
}
```

Two design choices to notice:

- `Pick` and `Remember` are `virtual` — the hard opponent will override them.
- `RandomChoice` and `CounterTo` are `protected` — helpers meant for this class *and its
  subclass*. The adaptive opponent will reuse both without rewriting them. This is the
  "share real implementation" half of inheritance.

`Remember` has an empty body on purpose. The contract says an opponent *can* be told a
throw; it does not say it has to *do* anything with it. The easy opponent honestly fulfills
the contract by ignoring the information.

## The hard opponent

`AdaptiveOpponent` inherits from `RandomOpponent` and overrides the two methods to use a
memory. Create `AdaptiveOpponent.cs`:

```csharp
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Random;

namespace JacksMod;

public class AdaptiveOpponent : RandomOpponent
{
    private const int MEMORY_SIZE = 3;
    private readonly List<Choice> recentThrows = new();

    public override void Remember(Choice playerThrow)
    {
        recentThrows.Add(playerThrow);
        if (recentThrows.Count > MEMORY_SIZE)
        {
            recentThrows.RemoveAt(0);
        }
    }

    public override Choice Pick()
    {
        if (recentThrows.Count == 0)
        {
            return RandomChoice();   // inherited from RandomOpponent
        }

        Choice predicted = MostFrequent(recentThrows);
        bool playToCounter = Rng.Chaotic.NextBool() || Rng.Chaotic.NextBool();

        return playToCounter
            ? CounterTo(predicted)   // inherited from RandomOpponent
            : RandomChoice();        // inherited from RandomOpponent
    }

    private Choice MostFrequent(List<Choice> throws)
    {
        Dictionary<Choice, int> counts = new();
        foreach (Choice c in throws)
        {
            if (!counts.ContainsKey(c)) counts[c] = 0;
            counts[c] = counts[c] + 1;
        }

        Choice best = throws[0];
        int bestCount = 0;
        foreach (var pair in counts)
        {
            if (pair.Value > bestCount)
            {
                best = pair.Key;
                bestCount = pair.Value;
            }
        }
        return best;
    }
}
```

Look at what `AdaptiveOpponent` did *not* write: `RandomChoice` and `CounterTo`. It inherits
them from `RandomOpponent` and calls them directly. The only things it adds are the memory
(`recentThrows`, `Remember`) and the smarter `Pick`. Inheritance let the adaptive opponent
say "I am a random opponent, except I pick using a memory" — and reuse everything else.

Notice the **two** `using` lines. `using` directives are per-file — a `using` in
`RandomOpponent.cs` does *not* carry over to `AdaptiveOpponent.cs`, even though both classes
live in the same namespace. Because `AdaptiveOpponent.Pick` calls `Rng.Chaotic.NextBool()`
directly, this file needs its own `using MegaCrit.Sts2.Core.Random;`. Forget it and the
compiler will tell you `Rng` does not exist — a small, common mistake, and a good reminder
that each file declares its own imports.

## The one-line swap

Now the event. The field type becomes the **interface**, and which concrete opponent you
build is the only thing that picks the difficulty:

```csharp
public sealed class RockPaperScissors : ModSmithEventModel
{
    private const int WIN_GOLD = 50;

    private readonly Scoreboard scoreboard = new();
    private readonly IOpponent opponent = new AdaptiveOpponent();   // hard mode
    //  swap to:        IOpponent opponent = new RandomOpponent();  // easy mode

    // GenerateInitialOptions, Play, DetermineOutcome — UNCHANGED from Lesson 6
}
```

That is the entire change to the event. `Play` still calls `opponent.Pick()` and
`opponent.Remember(playerChoice)` — those calls do not change, because both opponents honor
the `IOpponent` contract. The event is **polymorphic**: it plays against "an opponent" and
genuinely does not know which kind it got. Swapping `new AdaptiveOpponent()` for
`new RandomOpponent()` changes the whole feel of the encounter without touching a single
line of event logic.

Sit with why that is possible:

- The event depends on the **interface** (`IOpponent`), not a concrete class.
- Both opponents satisfy that interface, so either can be dropped in.
- The choice of which one is made in exactly one place — the field initializer.

That "depend on the contract, decide the concrete thing in one place" pattern is one of the
most useful ideas in all of software design. You will meet it again under names like
"dependency injection" and "programming to an interface." Here it bought you swappable
difficulty for the cost of one interface and one word in the field type.

## Build it

```
dotnet publish
```

Play with `AdaptiveOpponent` and confirm it still counters your favorite throw (the behavior
from Lesson 6, now living in a subclass). Then swap to `RandomOpponent`, rebuild, and feel
the difference: the easy opponent never adapts, no matter how you throw.

## Vocabulary

**Program to an interface** — Depending on a contract type (`IOpponent`) rather than a
concrete class, so implementations can be swapped freely.

**Subclass / base class** — `AdaptiveOpponent` is a subclass of the base `RandomOpponent`.

**Reuse via inheritance** — A subclass calling inherited (`protected`) members instead of
re-implementing them.

**Seam** — A single place where a decision (which opponent) is made, making the system easy
to change. The field initializer is the seam here.

## Things to look up

- "dependency injection" — the grown-up version of the one-line swap
- "C# abstract class" — what if `RandomOpponent` should not be usable on its own?
- "composition over inheritance" — a design debate worth knowing exists
