namespace MegaCrit.Sts2.Core.Random;

// STAND-IN, not real game code.
//
// In the actual mod, `Rng.Chaotic` is the global random source provided by Slay
// the Spire 2 (you saw it in CoinFlip: `Rng.Chaotic.NextBool()`). It is not
// available outside the game, so this file provides a minimal stand-in with the
// same shape, letting the verbatim lesson classes compile and run on their own.
//
// The opponent classes are byte-for-byte what the lessons tell you to write —
// only this stub is added so they can run without the game.
public static class Rng
{
    // Seeded so the demo's output is reproducible run to run. The real game's
    // Chaotic source is not seeded this way; only the stub is.
    public static readonly ChaoticSource Chaotic = new ChaoticSource(seed: 12345);
}

public sealed class ChaoticSource
{
    private readonly System.Random random;

    public ChaoticSource(int seed) => random = new System.Random(seed);

    public bool NextBool() => random.Next(2) == 0;
}
