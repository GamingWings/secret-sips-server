namespace SecretSipsServer.Models;

public class Game
{
    public string Code { get; set; } = Guid.NewGuid().ToString()[..6].ToUpper();
    public bool IsStarted { get; set; } = false;
    public int Rounds { get; set; }
    public int CurrentRound { get; set; } = 0;
    public int MinSecrets { get; set; }
    public int TimerLength { get; set; }
    public List<Player> Players { get; set; } = new();
}