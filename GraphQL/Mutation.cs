using SecretSipsServer.DAOs;
using SecretSipsServer.GraphQL.Inputs;
using SecretSipsServer.Models;

namespace SecretSipsServer.GraphQL;

public class Mutation
{
    public async Task<Game> CreateGameAsync(
        CreateGameInput input,
        [Service] IGameDAO dao)
    {
        var player = new Player
        {
            UserName = input.UserName,
            Secrets = new List<Secret>()
        };

        var game = new Game
        {
            Rounds = input.Rounds,
            MinSecrets = input.MinSecrets,
            TimerLength = input.TimerLength,
            Players = new List<Player> { player }
        };

        await dao.CreateGameAsync(game);
        return game;
    }
}
