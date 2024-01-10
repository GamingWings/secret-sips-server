using SecretSipsServer.Models;

namespace SecretSipsServer.DAOs;

public class GameDAO
{
    private static List<Game> games = new List<Game>();
    public async Task CreateGame(Game game)
    {
        games.Add(game);
    }

    public async Task<Game?> GetGame(string code)
    {
        return games.FirstOrDefault(game => game.Code == code);
    }

    public async Task DeleteGame(string code)
    {
        games.RemoveAll(game => game.Code == code);
    }
}