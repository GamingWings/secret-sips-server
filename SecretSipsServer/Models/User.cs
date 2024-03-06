using System.Net.WebSockets;
using System.Text.Json.Serialization;

namespace SecretSipsServer.Models;

public class User
{
    public string UserName { get; set; }
    public int Score { get; set; }
    public IEnumerable<Secret> Secrets { get; set; }
    [JsonIgnore]
    public WebSocket Socket { get; set;}
}