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

    // Read-only views of the counts, so the event can display them.
    public int PlayerWins   => playerWins;
    public int ComputerWins => computerWins;
    public int Draws        => draws;

    public string Summary() =>
        $"Score — you: {playerWins}, opponent: {computerWins} (draws: {draws})";
}
