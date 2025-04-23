using SecretSipsServer.Models;

namespace SecretSipsServer.DAOs;

public interface IGameDAO
{
    public void CreateGame(Game game);
    public Game? GetGame(string code);
    public void DeleteGame(string code);

}