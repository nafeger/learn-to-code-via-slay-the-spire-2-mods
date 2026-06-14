# 03 — Giving the Opponent a Memory

The opponent from Lesson 5 is a blank slate every round. Now you give it a memory: a list
of your recent throws. This chapter adds the *storage*; the next one uses it to make a
decision.

## A field that is a collection

In Lesson 5 you learned that an object remembers things in its fields. A field can hold a
collection just as easily as it holds an `int`. Add a `List<Choice>` field to `RpsOpponent`:

```csharp
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Random;

namespace JacksMod;

public class RpsOpponent
{
    private const int MEMORY_SIZE = 3;
    private readonly List<Choice> recentThrows = new();

    public void Remember(Choice playerThrow)
    {
        recentThrows.Add(playerThrow);

        if (recentThrows.Count > MEMORY_SIZE)
        {
            recentThrows.RemoveAt(0);   // forget the oldest
        }
    }

    public Choice Pick()
    {
        // still random for now — chapter 04 makes this use the memory
        if (Rng.Chaotic.NextBool()) return Choice.Rock;
        if (Rng.Chaotic.NextBool()) return Choice.Paper;
        return Choice.Scissors;
    }
}
```

The `using System.Collections.Generic;` line is what makes `List<>` and `Dictionary<>`
available. (Many projects include it automatically; add it if the compiler complains it
cannot find `List`.)

`recentThrows` is `readonly` for the same reason the scoreboard was in Lesson 5: the
opponent always uses the *same* list object, but the contents change freely as throws come
and go. `readonly` locks the reference, not the contents.

## The fixed-size window

The interesting line is the size cap:

```csharp
if (recentThrows.Count > MEMORY_SIZE)
{
    recentThrows.RemoveAt(0);
}
```

Without it, the list grows forever — after a hundred rounds it holds a hundred throws, and
the opponent reacts to ancient history as much as to your last move. You want it to react to
your *recent* tendency, so you keep only the last `MEMORY_SIZE` throws.

The mechanism: every time you `Add` a new throw to the end, if the list is now over the
limit, `RemoveAt(0)` drops the oldest from the front. The list slides along, always holding
at most the three most recent throws. This is a **sliding window** — a fixed-size view over
the most recent items.

Trace it with `MEMORY_SIZE = 3` as you throw Rock, Paper, Scissors, Rock:

| You throw | after `Add` | over limit? | after cap | window holds |
|-----------|-------------|-------------|-----------|--------------|
| Rock | `[R]` | no (1) | `[R]` | R |
| Paper | `[R, P]` | no (2) | `[R, P]` | R, P |
| Scissors | `[R, P, S]` | no (3) | `[R, P, S]` | R, P, S |
| Rock | `[R, P, S, R]` | yes (4) | `[P, S, R]` | P, S, R |

On the fourth throw the window slides: the oldest Rock falls off the front, the new Rock
joins the back. The list never exceeds three. Why three? It is a tuning choice — small
enough to react to your *recent* pattern, large enough to see one. You will experiment with
this number in the exercises.

## Wiring `Remember` into the event

The opponent only knows what it is told. The event has to hand it each throw. In `Play`, add
one line — and **order matters**:

```csharp
private async Task Play(Choice playerChoice)
{
    Choice computerChoice = opponent.Pick();   // 1. opponent picks, reacting to PAST throws
    opponent.Remember(playerChoice);           // 2. THEN it learns this round's throw
    Outcome outcome = DetermineOutcome(playerChoice, computerChoice);
    scoreboard.Record(outcome);

    // ... the win/lose/continue logic from Lesson 5, unchanged ...
}
```

`Pick()` runs **before** `Remember(playerChoice)`. That sequencing is deliberate: the
opponent must commit to its throw based on what it has seen *so far*, before it learns what
you just did. If you reversed the two lines, the opponent would "remember" your current
throw and then pick — effectively seeing your move before choosing its own. That is not
prediction, that is cheating, and it would feel unfair. Picking first, then learning, is
honest: it can only ever react to your past.

This ordering point is easy to get wrong and important to get right. Reading the two lines,
you should be able to say out loud: "it picks using old information, then files away the new
information for next time."

## Where the state lives now

Step back and notice the layering of memory you have built:

- The **event** holds the opponent and the scoreboard (Lesson 5).
- The **opponent** holds the list of recent throws (this chapter).
- The **scoreboard** holds the running counts (Lesson 5).

Each object remembers exactly what it is responsible for, and nothing it is not. The
opponent does not know the score; the scoreboard does not know your throw history. Each
piece of state lives with the object whose job needs it. That separation is what keeps each
class small enough to understand on its own — the payoff of the encapsulation idea from
Lesson 5.

## Build and verify the wiring

Build it:

```
dotnet publish
```

The opponent still picks randomly, so you will not *feel* a difference yet — but the memory
is now being filled every round. The next chapter spends that memory.

## Vocabulary

**Sliding window** — A fixed-size view that always holds the most recent N items; old items
fall off as new ones arrive.

**Capacity cap** — The check that drops the oldest item once a collection exceeds its
intended size.

**Ordering dependency** — When two steps must run in a specific order to be correct.
`Pick()` before `Remember()` is one.

## Things to look up

- "sliding window algorithm" — the general pattern, used well beyond games
- "C# List RemoveAt vs Remove" — removing by position versus by value
- "circular buffer" — a more efficient fixed-size structure you could use later
