using SecretSipsServer.Models;
using System.Collections.Concurrent;

namespace SecretSipsServer.DAOs;

public class MemoryGameDAO : IGameDAO
{
    private static readonly ConcurrentDictionary<string, Game> games = new();

    public Task CreateGameAsync(Game game)
    {
        games[game.Code] = game;
        return Task.CompletedTask;
    }

    public Task<Game?> GetGameAsync(string code)
    {
        games.TryGetValue(code, out var game);
        return Task.FromResult(game);
    }

    public Task DeleteGameAsync(string code)
    {
        games.TryRemove(code, out _);
        return Task.CompletedTask;
    }
}
