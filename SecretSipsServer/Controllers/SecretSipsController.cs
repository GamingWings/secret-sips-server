using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SecretSipsServer.DAOs;
using SecretSipsServer.Models;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;


namespace SecretSipsServer.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("[controller]")]
public class SecretSipsController : ControllerBase
{
    private readonly ILogger<SecretSipsController> logger;
    private readonly Random random = new Random();
    private readonly GameDAO dao;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dao"></param>
    public SecretSipsController(ILogger<SecretSipsController> logger, GameDAO dao)
    {
        this.logger = logger;
        this.dao = dao;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("Create")]
    public async Task Create([FromQuery] CreateGameRequest request)
    {
        var code = await GenerateCode();
        var game = new Game
        {
            Code = code,
            Rounds = request.Rounds,
            IsStarted = false,
            Users = new List<User>(),
            CurrentRound = 1,
            MinSecrets = request.MinSecrets,
            TimerLength = request.TimerLength
        };
        await dao.CreateGame(game);
        var loop = GameLoop(game, new User {UserName = request.UserName});
        loop.Wait();
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet("Join")]
    public async Task Join([FromQuery] string UserName, [FromQuery] string Code)
    {
        logger.LogInformation("Received new connection");
        var game = await dao.GetGame(Code);
        if (game == null) 
        {
            logger.LogWarning("Game does not exist");
            HttpContext.Response.StatusCode = 400;
            return;
        }
        await GameLoop(game, new User{UserName = UserName});
    }

    private async Task<string> GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Game? game = null;
        var code = "";
        do
        {
            code = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            game = await dao.GetGame(code);
        } while (game != null);

        return code;

    } 

    private async Task GameLoop(Game game, User user) 
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            logger.LogInformation("Connection is Websocket");
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            game.Users.Append(user);
            var message = Encoding.UTF8.GetBytes("message");
            game.Users.ToList().ForEach(async user => await user.Socket.SendAsync(message, WebSocketMessageType.Text, false, CancellationToken.None));
            while (true)
            {
                var messageTask = await GetMessage(webSocket);
                messageTask.Wait();
            }
            // Draw the rest of the fucking owl
        }
        else
        {
            logger.LogWarning("Connection is not a Websocket");
            HttpContext.Response.StatusCode = 400;
        }
    }

    private static async Task<dynamic> GetMessage(WebSocket socket)
    {
        var message = "";
        var buffer = new byte[1024 * 4];
        while (true) {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                break;
            }
            message += Encoding.UTF8.GetString(buffer, 0, result.Count);
            if (result.EndOfMessage) {
                break;
            }
        }
        return JsonSerializer.Deserialize<dynamic>(message);
    }
}