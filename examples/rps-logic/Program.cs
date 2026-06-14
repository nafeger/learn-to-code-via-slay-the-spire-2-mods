using System;
using JacksMod;

// A demo harness for the framework-independent RPS logic from Lessons 5-7.
// It does NOT use the game; it drives the same Scoreboard and opponent classes
// the lessons build, and checks that they behave the way the lessons claim.
//
// Run with:  dotnet run --project examples/rps-logic

internal static class Program
{
    // In the real mod, DetermineOutcome lives inside the RockPaperScissors event
    // (Lesson 4, carried into Lesson 5). It is pure logic, so the demo can hold a
    // copy to score rounds without the event system.
    private static Outcome DetermineOutcome(Choice player, Choice computer)
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

    private static int Main()
    {
        bool allPassed = true;

        Console.WriteLine("=== RPS logic demo (Lessons 5-7) ===\n");

        allPassed &= DemoBestOfThreeMatch();
        allPassed &= DemoScoreboardTracksDraws();
        allPassed &= DemoAdaptiveCountersASpammer();
        allPassed &= DemoRandomDoesNotAdapt();

        Console.WriteLine();
        Console.WriteLine(allPassed ? "ALL CHECKS PASSED" : "SOME CHECKS FAILED");
        return allPassed ? 0 : 1;
    }

    // Lesson 5: a best-of-three match runs to completion and the scoreboard
    // remembers the running tally across rounds.
    private static bool DemoBestOfThreeMatch()
    {
        Console.WriteLine("[Lesson 5] Best-of-three match against the adaptive opponent:");

        Scoreboard scoreboard = new();
        IOpponent opponent = new AdaptiveOpponent();
        int round = 0;

        // The "player" here just keeps throwing Rock, to keep the demo deterministic.
        while (!scoreboard.PlayerWonMatch() && !scoreboard.ComputerWonMatch())
        {
            round++;
            Choice playerChoice = Choice.Rock;
            Choice computerChoice = opponent.Pick();
            opponent.Remember(playerChoice);

            Outcome outcome = DetermineOutcome(playerChoice, computerChoice);
            scoreboard.Record(outcome);

            Console.WriteLine(
                $"  round {round}: you={playerChoice}, opponent={computerChoice} -> {outcome}");
        }

        string winner = scoreboard.PlayerWonMatch() ? "player" : "opponent";
        Console.WriteLine($"  match over after {round} rounds. {scoreboard.Summary()} -> {winner} wins\n");

        bool ok = round >= 2 && (scoreboard.PlayerWonMatch() ^ scoreboard.ComputerWonMatch());
        Report("best-of-three reaches a single winner", ok);
        return ok;
    }

    // Lesson 5: the scoreboard counts wins, losses, AND draws separately, and
    // reports all three. (The best-of-three demo above never produces a draw with
    // this seed, so this check covers the draw path the "W/L/D score line" promises.)
    private static bool DemoScoreboardTracksDraws()
    {
        Scoreboard scoreboard = new();
        scoreboard.Record(Outcome.Win);
        scoreboard.Record(Outcome.Lose);
        scoreboard.Record(Outcome.Draw);

        string summary = scoreboard.Summary();
        Console.WriteLine($"[Lesson 5] Scoreboard after one win, one loss, one draw: {summary}");

        bool ok = summary.Contains("you: 1")
            && summary.Contains("opponent: 1")
            && summary.Contains("draws: 1")
            && !scoreboard.PlayerWonMatch()      // one win is not a match win
            && !scoreboard.ComputerWonMatch();
        Report("scoreboard counts wins, losses, and draws separately", ok);
        return ok;
    }

    // Lesson 6/7: against a player who always throws Rock, the adaptive opponent
    // should throw Paper (the counter) far more than a third of the time.
    private static bool DemoAdaptiveCountersASpammer()
    {
        IOpponent opponent = new AdaptiveOpponent();
        int paper = CountCounterThrows(opponent, spam: Choice.Rock, counter: Choice.Paper, rounds: 1000);
        double rate = paper / 1000.0;

        Console.WriteLine($"[Lesson 6/7] Adaptive vs a Rock-spammer: threw Paper {rate:P0} of the time");
        bool ok = rate > 0.5;   // a non-adaptive opponent would sit near 33%
        Report("adaptive opponent counters a predictable player (Paper > 50%)", ok);
        return ok;
    }

    // Lesson 7: the random opponent ignores history, so its Paper rate stays near
    // a third no matter what the player does.
    private static bool DemoRandomDoesNotAdapt()
    {
        IOpponent opponent = new RandomOpponent();
        int paper = CountCounterThrows(opponent, spam: Choice.Rock, counter: Choice.Paper, rounds: 1000);
        double rate = paper / 1000.0;

        Console.WriteLine($"[Lesson 7] Random vs a Rock-spammer: threw Paper {rate:P0} of the time");
        bool ok = rate < 0.45;   // never adapts; stays near the unbiased ~33%
        Report("random opponent does NOT adapt (Paper < 45%)", ok);
        return ok;
    }

    private static int CountCounterThrows(IOpponent opponent, Choice spam, Choice counter, int rounds)
    {
        int hits = 0;
        for (int i = 0; i < rounds; i++)
        {
            Choice pick = opponent.Pick();
            opponent.Remember(spam);
            if (pick == counter) hits++;
        }
        return hits;
    }

    private static void Report(string label, bool ok)
        => Console.WriteLine($"    [{(ok ? "PASS" : "FAIL")}] {label}");
}
