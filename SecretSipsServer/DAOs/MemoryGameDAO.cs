using SecretSipsServer.Models;

namespace SecretSipsServer.DAOs;

public class MemoryGameDAO : IGameDAO
{
    private static readonly List<Game> games = [];
    public void CreateGame(Game game)
    {
        games.Add(game);
    }

    public Game? GetGame(string code)
    {
        return games.FirstOrDefault(game => game.Code == code);
    }

    public void DeleteGame(string code)
    {
        games.RemoveAll(game => game.Code == code);
    }
}