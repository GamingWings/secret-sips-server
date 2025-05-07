using SecretSipsServer.Models;

namespace SecretSipsServer.DAOs;

public interface IGameDAO
{
    Task CreateGameAsync(Game game);
    Task<Game?> GetGameAsync(string code);
    Task DeleteGameAsync(string code);
}
