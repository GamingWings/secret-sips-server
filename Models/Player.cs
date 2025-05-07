namespace SecretSipsServer.Models;

public class Player
{
    public string UserName { get; set; }
    public int Score { get; set; }
    public IEnumerable<Secret> Secrets { get; set; }
}
