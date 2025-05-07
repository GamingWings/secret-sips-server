namespace SecretSipsServer.GraphQL.Inputs;

public class CreateGameInput
{
    public int Rounds { get; set; }
    public int MinSecrets { get; set; }
    public int TimerLength { get; set; }
    public string UserName { get; set; } = default!;
}
