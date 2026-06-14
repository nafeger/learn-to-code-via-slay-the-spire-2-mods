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
        foreach (var pair in counts)
        {
            if (pair.Value > counts[best]) best = pair.Key;
        }
        return best;
    }
}
