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
