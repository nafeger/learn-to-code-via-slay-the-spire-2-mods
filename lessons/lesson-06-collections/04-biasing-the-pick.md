# 04 — Biasing the Pick

The opponent has a memory. Now it uses it. By the end of this chapter, throwing the same
move repeatedly will get punished.

## The plan in plain words

1. If the opponent has no memory yet (first round), pick at random.
2. Otherwise, find the throw you have made *most often* recently.
3. Most of the time, throw the move that **beats** that. Some of the time, pick at random so
   it stays unpredictable.

Step 2 is the `MostFrequent` tally from chapter 02. Step 3 reuses the weighted-coin idea
from Lessons 2 and 3 — bias a decision without needing perfect probabilities.

## Countering a throw

First, a tiny helper: given a throw, what beats it?

```csharp
private Choice CounterTo(Choice c)
{
    return c switch
    {
        Choice.Rock     => Choice.Paper,     // paper beats rock
        Choice.Paper    => Choice.Scissors,  // scissors beats paper
        Choice.Scissors => Choice.Rock,      // rock beats scissors
        _               => Choice.Rock,
    };
}
```

This is the same switch-expression shape as `DetermineOutcome` from Lesson 4, but simpler:
one input, one output. If you throw Rock most, the counter is Paper.

## The biased Pick

Here is the full opponent with `Pick` rewritten to use the memory:

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
            recentThrows.RemoveAt(0);
        }
    }

    public Choice Pick()
    {
        if (recentThrows.Count == 0)
        {
            return RandomChoice();          // no memory yet — play fair
        }

        Choice predicted = MostFrequent(recentThrows);

        // Throw the counter most of the time, but not always (~75%).
        // Two coin flips: true unless BOTH come up false.
        bool playToCounter = Rng.Chaotic.NextBool() || Rng.Chaotic.NextBool();

        return playToCounter ? CounterTo(predicted) : RandomChoice();
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

    private Choice CounterTo(Choice c) => c switch
    {
        Choice.Rock     => Choice.Paper,
        Choice.Paper    => Choice.Scissors,
        Choice.Scissors => Choice.Rock,
        _               => Choice.Rock,
    };

    private Choice RandomChoice()
    {
        if (Rng.Chaotic.NextBool()) return Choice.Rock;
        if (Rng.Chaotic.NextBool()) return Choice.Paper;
        return Choice.Scissors;
    }
}
```

### Why not counter 100% of the time?

If the opponent *always* threw the counter to your favorite, you would beat it instantly:
just figure out it is countering, and throw the move that beats *the counter*. A perfectly
predictable opponent is as easy to exploit as a perfectly random one is boring. Mixing —
counter often, but sometimes throw random — keeps it from being readable. This is the same
"bias, do not guarantee" lesson as the weighted coin: a tendency, not a certainty.

### Reading the probability

```csharp
bool playToCounter = Rng.Chaotic.NextBool() || Rng.Chaotic.NextBool();
```

`||` is "or," and it short-circuits: if the first `NextBool()` is `true`, the second is not
even evaluated. So `playToCounter` is `false` only when *both* flips come up `false` — a 1-
in-4 chance (25%). That makes it `true` 75% of the time. The opponent throws the counter
about three rounds in four, and mixes in a random pick the other one in four. You worked out
this exact "two coin flips" probability in Lesson 3; here it is doing real work.

If you want a different mix, this one line is the dial. A single `NextBool()` would be 50/50.
Three `||`'d together would be 87.5%. Pick the aggressiveness you want.

## See it adapt

Build and play:

```
dotnet publish
```

Now the scoreboard from Lesson 5 earns its keep. Try this in the event:

- Throw **Rock every round**. The first round is fair (no memory). By the second or third,
  the opponent's memory fills with Rock, `MostFrequent` returns Rock, and it starts throwing
  Paper about 75% of the time. Your win count stalls; its win count climbs. You can *see* the
  adaptation in the running score.
- Now **mix up** your throws. With no clear favorite in your recent window, `MostFrequent`
  has nothing strong to counter, and you do better. Varying your play beats a predictable
  pattern — which is exactly true of real Rock Paper Scissors.

That feedback loop — your behavior changes the opponent's behavior, and the scoreboard shows
the result — is the whole feature. It exists because the opponent can *remember* (a list) and
*summarize* (a dictionary tally) and *decide* (the weighted pick). Three collection ideas,
one satisfying result.

## What you just built

The opponent reads your recent throws, tallies your favorite, and biases its pick to beat
it — while staying unpredictable enough to be fair. You used a list to remember, a dictionary
to count, and a weighted coin to decide. None of it required new game-framework knowledge:
it is ordinary C# collections doing the work.

## Vocabulary

**Bias (in selection)** — Nudging a random choice toward one outcome without forcing it.

**Short-circuit evaluation** — `||` stops at the first `true`; `&&` stops at the first
`false`. The rest is not evaluated.

**Exploitable** — A strategy predictable enough that an opponent can reliably beat it. Both
"always counter" and "always Rock" are exploitable.

**Feedback loop** — When the output of a system (the opponent's adaptation) depends on its
own recent input (your throws).

## Things to look up

- "C# logical operators short circuit" — the precise rules for `||` and `&&`
- "rock paper scissors AI" — real approaches to predicting an opponent (frequency,
  history-matching); yours is the simplest useful one
- "C# LINQ" — `recentThrows.GroupBy(...)` can do the tally in one line, once you are ready
