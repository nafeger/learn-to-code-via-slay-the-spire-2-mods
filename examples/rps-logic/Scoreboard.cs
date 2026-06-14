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
