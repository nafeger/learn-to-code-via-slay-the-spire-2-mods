# 04 — The Opponent as an Object

You have a `Scoreboard` class. Now turn the computer player into an object too, and wire
both into a best-of-three match.

## Why the opponent should be an object

In Lesson 4, the computer's choice came from a method on the event:

```csharp
private Choice PickComputerChoice()
{
    if (Rng.Chaotic.NextBool()) return Choice.Rock;
    if (Rng.Chaotic.NextBool()) return Choice.Paper;
    return Choice.Scissors;
}
```

It works, but it is stuck. The computer has no identity and no memory — it is just a
function on the event. In Lesson 6 you are going to give the opponent a *memory* of your
recent throws so it can start predicting you. A method with no state cannot hold a memory.
An object can.

So you extract the opponent into its own class now, while it is still simple. This is a
common move: pull a responsibility out into its own object *before* it needs to grow, so
there is a clean place for the growth to happen.

## The RpsOpponent class

Create `src/StarterContent/events/RpsOpponent.cs`:

```csharp
using MegaCrit.Sts2.Core.Random;

namespace JacksMod;

public class RpsOpponent
{
    public Choice Pick()
    {
        if (Rng.Chaotic.NextBool()) return Choice.Rock;
        if (Rng.Chaotic.NextBool()) return Choice.Paper;
        return Choice.Scissors;
    }
}
```

For now this is just the old method in a new home. `Pick()` still has the same Rock-50% /
Paper-25% / Scissors-25% bias you analyzed in Lesson 4 — still good enough for now, and
Lesson 9 still owns proper uniform randomness. The point is not new behavior; it is that
the opponent is now a *thing* with a clear place to keep state. In Lesson 6, that empty
class body fills up with a memory.

> **`Rng.Chaotic.NextBool()`** — this is the global random-number source, the same one
> `CoinFlip` uses (`Rng.Chaotic.NextBool()`). Because `RpsOpponent` is a plain class and
> not an event, it reaches for the global RNG directly rather than an event-provided one.
> The `using MegaCrit.Sts2.Core.Random;` at the top is what makes `Rng` available.

## Rewiring the event

Now the event *has a* scoreboard and *has an* opponent. Both are fields, because both must
survive across the rounds of a match. Here is the full `RockPaperScissors` for this lesson:

```csharp
using ModSmith.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace JacksMod;

public sealed class RockPaperScissors : ModSmithEventModel
{
    private const int WIN_GOLD = 50;

    private readonly Scoreboard scoreboard = new();
    private readonly RpsOpponent opponent = new();

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new GoldVar(WIN_GOLD),
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return [
            new EventOption(this, () => Play(Choice.Rock),     "ROCK_PAPER_SCISSORS.options.ROCK"),
            new EventOption(this, () => Play(Choice.Paper),    "ROCK_PAPER_SCISSORS.options.PAPER"),
            new EventOption(this, () => Play(Choice.Scissors), "ROCK_PAPER_SCISSORS.options.SCISSORS"),
        ];
    }

    private async Task Play(Choice playerChoice)
    {
        Choice computerChoice = opponent.Pick();
        Outcome outcome = DetermineOutcome(playerChoice, computerChoice);
        scoreboard.Record(outcome);

        if (Owner is Player player)
        {
            if (scoreboard.PlayerWonMatch())
            {
                await PlayerCmd.GainGold(WIN_GOLD, player);
                SetEventFinished(L10NLookup("ROCK_PAPER_SCISSORS.pages.WIN.description"));
            }
            else if (scoreboard.ComputerWonMatch())
            {
                SetEventFinished(L10NLookup("ROCK_PAPER_SCISSORS.pages.LOSE.description"));
            }
            else
            {
                SetEventState(scoreboard.Summary(), GenerateInitialOptions());
            }
        }
    }

    private Outcome DetermineOutcome(Choice player, Choice computer)
    {
        if (player == computer) return Outcome.Draw;

        return (player, computer) switch
        {
            (Choice.Rock,     Choice.Scissors) => Outcome.Win,
            (Choice.Paper,    Choice.Rock)     => Outcome.Win,
            (Choice.Scissors, Choice.Paper)    => Outcome.Win,
            _                                  => Outcome.Lose,
        };
    }
}
```

`PickComputerChoice` is gone from the event — its job moved to `opponent.Pick()`. The
enums are gone too; they live in `RpsTypes.cs` now. `DetermineOutcome` stays, because
comparing two choices is the event's rule, not the opponent's behavior.

### The two fields are the whole lesson

```csharp
private readonly Scoreboard scoreboard = new();
private readonly RpsOpponent opponent = new();
```

These two lines create the objects when the event object is created, and hold onto them
for the event's whole life. Because they are **fields**, not locals, they persist across
every round the player plays. The `new()` on the right is C#'s shorthand for
`new Scoreboard()` — the type is already stated on the left, so you can leave it off.

`readonly` means the field always points at the *same* scoreboard object for the life of
the event. You can still change what is *inside* the scoreboard (record wins all day) — you
just cannot swap it out for a different scoreboard. That matches reality: one match, one
scoreboard.

### How best-of-three works with no loop

There is no `for` or `while` here, yet the match loops. The mechanism is the same one
Lesson 4's draw used: each round, if nobody has won the match yet, the `else` branch calls

```csharp
SetEventState(scoreboard.Summary(), GenerateInitialOptions());
```

which shows the running score and puts the three buttons back. The player clicks again,
`Play` runs again, `scoreboard.Record(...)` updates the *same* scoreboard field — and the
tally climbs because the object remembered it. The loop is driven by the player's clicks
and the persistent object, not by loop syntax. When `PlayerWonMatch()` or
`ComputerWonMatch()` finally returns true, `SetEventFinished` ends it.

This is worth sitting with: the score survives between clicks **only because** it lives on
a field of an object that the event holds. Put it in a local variable inside `Play` and it
would reset to zero every round. The whole best-of-three feature rests on the
class/object idea from this lesson.

## Register and build

`RockPaperScissors` is already registered (`Registry.RegisterEvent<RockPaperScissors>();`).
The new classes — `Scoreboard`, `RpsOpponent`, and the `RpsTypes.cs` enums — do **not**
need registration. Registration is only for *content the game places into a run* (cards,
relics, events). Your helper classes are plain objects your event uses internally; the game
does not need to know they exist.

Build from the mod directory:

```
dotnet publish
```

## What you just built

Rock Paper Scissors is now a best-of-three match against an opponent object, with a
scoreboard object that remembers the running tally across rounds. You designed two classes,
each bundling its own state with the behavior that acts on it, and the event composes them.

The opponent still picks at random. That changes next: in Lesson 6 the opponent grows a
memory of your throws.

## Vocabulary

**Field initializer** — Setting a field's value where it is declared:
`private readonly Scoreboard scoreboard = new();`.

**`readonly`** — A field whose reference cannot be reassigned after construction. The
object it points to can still change internally.

**Composition ("has a")** — Building behavior by having an object hold other objects. The
event *has a* scoreboard and *has an* opponent.

**`new()` (target-typed new)** — Shorthand for `new TypeName()` when the type is already
known from the left-hand side.

## Things to look up

- "C# readonly field" — what it does and does not prevent
- "C# target-typed new" — the `new()` shorthand
- "object composition vs inheritance" — a preview of a Lesson 7 theme
