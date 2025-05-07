namespace SecretSipsServer.Models;

public class Secret
{
    public string Text { get; set; } = default!;
    public bool IsUsed { get; set; } = false;
}