# 04 — The Opponent as an Object

You have a `Scoreboard` class. Now turn the computer player into an object too, and wire
both into a best-of-three match.

## Why the opponent should be an object

In Lesson 4, the computer's choice came from a method on the event:

```csharp
private Choice PickComputerChoice()
{
    if (Rng.NextBool()) return Choice.Rock;
    if (Rng.NextBool()) return Choice.Paper;
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

This is the old method in a new home, with **one change**: the Lesson 4 version called
the bare `Rng.NextBool()`, and here it is `Rng.Chaotic.NextBool()`. That change is forced,
not cosmetic — see the note below. Otherwise the logic is identical: `Pick()` still has the
same Rock-50% / Paper-25% / Scissors-25% bias you analyzed in Lesson 4 — still good enough
for now. (If that lopsided shape bugs you, you have the right instinct: chaining two coin
flips is not the natural way to pick one of three things. The cleaner approach is to draw a
*single* random number and map it onto the three choices — that is what Lesson 9 does, along
with why even that has a subtle trap. We keep the coin-flip version here only because it uses
nothing you have not already seen.) The point is not new behavior; it is that the opponent is
now a *thing* with a clear place to keep state. In Lesson 6, that empty class body fills up
with a memory.

> **Why `Rng.NextBool()` became `Rng.Chaotic.NextBool()`** — in Lesson 4, `PickComputerChoice`
> lived *inside the event*, and the bare `Rng` was a convenience the `ModSmithEventModel` base
> class provided (you will see how that inheritance works in Lesson 7). `RpsOpponent` is a
> plain class, not an event, so it does not inherit that convenience. It has to reach for the
> global random source directly: `Rng.Chaotic` — the same global generator the game falls back
> to elsewhere (you saw it in `CoinFlip`). The `using MegaCrit.Sts2.Core.Random;` at the top
> is what makes `Rng` available. Move event code into a plain class and you will hit exactly
> this kind of "that helper isn't here anymore" adjustment — it is a normal part of extracting.

## Rewiring the event

Now the event *has a* scoreboard and *has an* opponent. Both are fields, because both must
survive across the rounds of a match. Here is the full `RockPaperScissors` for this lesson:

```csharp
using ModSmith.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization;          // LocString
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
                LocString score = L10NLookup("ROCK_PAPER_SCISSORS.pages.SCORE.description");
                score.Add("playerWins",   scoreboard.PlayerWins);
                score.Add("computerWins", scoreboard.ComputerWins);
                score.Add("draws",        scoreboard.Draws);
                SetEventState(score, GenerateInitialOptions());
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
LocString score = L10NLookup("ROCK_PAPER_SCISSORS.pages.SCORE.description");
score.Add("playerWins",   scoreboard.PlayerWins);
score.Add("computerWins", scoreboard.ComputerWins);
score.Add("draws",        scoreboard.Draws);
SetEventState(score, GenerateInitialOptions());
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

## Showing the score the translatable way

You might expect to hand `SetEventState` the string from `Scoreboard.Summary()`. You cannot,
and the compiler will stop you:

```csharp
// Does NOT compile — Summary() is a string, SetEventState wants a LocString.
SetEventState(scoreboard.Summary(), GenerateInitialOptions());
```

`SetEventState` does not take a `string`. It takes a `LocString` — the game's *localized*
text type. A `LocString` is not the finished text; it is a **reference to a key** in your
`events.json` plus any dynamic values you attach. The game resolves it to actual text at
display time, in whatever language the player is running. A raw C# string can't do that —
it is frozen English — so there is no automatic conversion, which is why the line above is a
compile error rather than a silent mistake.

That is also why the score has to be built from a key. Two steps:

```csharp
LocString score = L10NLookup("ROCK_PAPER_SCISSORS.pages.SCORE.description");
score.Add("playerWins",   scoreboard.PlayerWins);
score.Add("computerWins", scoreboard.ComputerWins);
score.Add("draws",        scoreboard.Draws);
```

1. `L10NLookup(key)` returns a `LocString` pointing at the `SCORE` text in `events.json`.
2. `.Add(name, value)` attaches a **dynamic variable** — a live number the text can drop in.
   This is the same mechanism behind `{Gold}` from Lesson 4, but supplied at runtime instead
   of from `CanonicalVars`. Each round you call `Play`, you read the *current* counts off the
   persistent `scoreboard` and add them, so the displayed score climbs with the match.

Add the matching key to `JacksMod/localization/eng/events.json`, using `{...}` tokens whose
names match the strings you passed to `.Add(...)`:

```json
"ROCK_PAPER_SCISSORS.pages.SCORE.description": "Score so far -- you: {playerWins}, opponent: {computerWins} (draws: {draws}). Best of three; pick again."
```

Now the score line is translatable (every language gets its own `SCORE` text) *and* dynamic
(the `{playerWins}` token shows whatever number you added this round). `Summary()` is still
fine for a `Console.WriteLine` while you debug — it just isn't what the player sees.

> **Remember the prefix rule.** The key still begins with `ROCK_PAPER_SCISSORS` because that
> is the class name in SCREAMING_SNAKE_CASE. A mismatch here is the most common cause of a
> bare `NullReferenceException` from an event — see `MODDING.md` for the full troubleshooting
> note.

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

## Vetting the logic without launching the game

Notice what just happened: `Scoreboard`, `RpsOpponent`, and `DetermineOutcome` are **plain
C# classes**. They don't touch `LocString`, `SetEventState`, or anything from the game. Only
the thin shell — `Play`'s branching and the `LocString` it builds — needs the game at all.

That split is worth designing for on purpose, because **plain logic can be run and checked
without starting Slay the Spire 2.** This repo ships a tiny console harness that does exactly
that:

```
dotnet run --project examples/rps-logic
```

It drives the same `Scoreboard` and opponent classes and asserts they behave as the lessons
claim (the scoreboard counts wins/losses/draws, a best-of-three reaches one winner, an
adaptive opponent counters a predictable player). The one game touchpoint those classes need —
`Rng.Chaotic` — is replaced with a small stand-in in `GameStubs.cs`. No Steam, no game window,
a sub-second feedback loop.

The boundary is real: the harness **cannot** vet the `LocString` / `SetEventState` wiring,
because those types live in the game's `sts2.dll`. That layer only truly proves out in-game.
The lesson there is to keep decisions in plain classes (cheap to check headless) and keep the
game-framework shell as thin as you can — the more logic you pull into objects like
`Scoreboard`, the more of your mod you can verify before ever loading the game.

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
