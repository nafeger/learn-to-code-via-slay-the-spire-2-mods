namespace JacksMod;

public interface IOpponent
{
    Choice Pick();
    void Remember(Choice playerThrow);
}
