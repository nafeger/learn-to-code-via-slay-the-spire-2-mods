# 03 — Building the Scoreboard

Now you write a class from scratch. The `Scoreboard` is the simplest possible real class —
state plus behavior, nothing else — which makes it the perfect first one.

## What it needs to do

Stated plainly, a scoreboard for a best-of-three match:

1. Remembers how many rounds the player has won, the computer has won, and how many were
   draws.
2. Records the result of a round.
3. Reports whether someone has won the match (first to two).
4. Describes the current score as text to show the player.

That description names the fields and the methods, exactly like the method-design process
in Lesson 4 — except now the data and the behavior live together in one class.

## Sharing the `Outcome` type

There is a snag to handle first. In Lesson 4, `Outcome` was declared *inside*
`RockPaperScissors`:

```csharp
private enum Outcome { Win, Lose, Draw }
```

The `Scoreboard` needs to talk about outcomes too — `Record` takes an `Outcome`. But a
`private` enum nested inside `RockPaperScissors` is not visible to other classes. When two
classes need to share a type, that type no longer belongs to just one of them — it moves
*out* to the namespace, where both can see it.

Create a small file, `src/StarterContent/events/RpsTypes.cs`, and move both enums there:

```csharp
namespace JacksMod;

public enum Choice { Rock, Paper, Scissors }
public enum Outcome { Win, Lose, Draw }
```

They were `private` and nested; now they are `public` and live at the namespace level.
("Namespace" is the `namespace JacksMod;` line at the top of every file — chapter 05 explains
it fully. For now, read it as "the shared space all your mod's types live in.") Delete the two
nested `enum` lines from `RockPaperScissors`. Nothing else about the event changes —
`Choice.Rock` and `Outcome.Win` still mean the same thing; they just live somewhere both the
event and the new classes can reach.

> This is a recurring pattern: a type starts life owned by one class, then a second class
> needs it, so it graduates to a shared location. Moving it is the right response, not a
> workaround.

## The Scoreboard class

Create `src/StarterContent/events/Scoreboard.cs`:

```csharp
namespace JacksMod;

public class Scoreboard
{
    private const int WINS_NEEDED = 2;   // best of three: first to two wins

    private int playerWins;
    private int computerWins;
    private int draws;

    public void Record(Outcome outcome)
    {
        if (outcome == Outcome.Win)       playerWins++;
        else if (outcome == Outcome.Lose) computerWins++;
        else                              draws++;
    }

    public bool PlayerWonMatch()   => playerWins   >= WINS_NEEDED;
    public bool ComputerWonMatch() => computerWins >= WINS_NEEDED;

    public string Summary() =>
        $"Score — you: {playerWins}, opponent: {computerWins} (draws: {draws})";
}
```

Read it as state plus behavior:

- **State**: three private `int` fields, plus a `const` for the rule "first to two."
- **Behavior**: `Record` changes the state; `PlayerWonMatch`/`ComputerWonMatch` ask
  questions about it; `Summary` describes it.

### The pieces you have not seen as your own code

**Expression-bodied methods.** `public bool PlayerWonMatch() => playerWins >= WINS_NEEDED;`
is shorthand for a method whose whole body is one expression. It is identical to:

```csharp
public bool PlayerWonMatch()
{
    return playerWins >= WINS_NEEDED;
}
```

The `=>` form is the same thing the mod's own code uses — `CanonicalVars => [...]` in
`CoinFlip` is an expression-bodied member. Use whichever reads more clearly; for one-line
methods, `=>` is common.

**String interpolation.** `$"Score — you: {playerWins}, ..."` is an interpolated string.
The `$` in front lets you drop `{playerWins}` into the text and have its value substituted
in. You saw this exact feature in `CoinFlip`: `$"{base.Id.Entry}.headsBanter"`.

### Why `Record` controls everything

`Record` is the *only* way the counts change, and it is the only place that decides which
count an outcome maps to. Because the fields are private, no other code can increment
`playerWins` directly. If you later change the rules — say a draw secretly counts as half a
point for someone — you change `Record` and nowhere else. That is encapsulation paying off:
one class owns its data and the rules for changing it.

## Trace it by hand

Before wiring it in, confirm you understand it. Walk a best-of-three where the player wins,
loses, then wins:

| Call | `playerWins` | `computerWins` | `draws` | `PlayerWonMatch()` |
|------|--------------|----------------|---------|--------------------|
| start | 0 | 0 | 0 | false |
| `Record(Outcome.Win)` | 1 | 0 | 0 | false |
| `Record(Outcome.Lose)` | 1 | 1 | 0 | false |
| `Record(Outcome.Win)` | 2 | 1 | 0 | **true** |

After the third round, `PlayerWonMatch()` returns `true` and the match is over. The object
remembered the running tally across three separate calls — which is the thing a lone method
could never do.

## Vocabulary

**Expression-bodied method** — A method written with `=>` when its body is a single
expression. `bool Done() => count >= 2;`

**String interpolation** — Building a string with `$"...{value}..."`, substituting values
inline.

**Single source of truth** — Keeping the rule for changing data in exactly one place
(here, `Record`), so a change has one home.

## Things to look up

- "C# string interpolation" — formatting numbers, alignment, the full syntax
- "C# expression-bodied members" — where `=>` is allowed
- "C# enum" — review from Lesson 4, now that enums live at namespace scope
