namespace SecretSipsServer.Models;

public class Game
{
    public string Code { get; set; }
    public bool IsStarted { get; set; }
    public int Rounds { get; set; }
    public int CurrentRound { get; set; }
    public int MinSecrets { get; set; }
    public int TimerLength { get; set; }
    public IEnumerable<User> Users { get; set; }
}