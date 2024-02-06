using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SecretSipsServer.DAOs;
using SecretSipsServer.Models;

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
    [HttpPost("Create")]
    public async Task<ActionResult> Create([FromBody] CreateGameRequest request)
    {
        var code = await GenerateCode();
        var game = new Game
        {
            Code = code,
            Rounds = request.Rounds,
            IsStarted = false,
            Users = new List<User>{new User {UserName = request.UserName}},
            CurrentRound = 1,
            MinSecrets = request.MinSecrets,
            TimerLength = request.TimerLength
        };
        await dao.CreateGame(game);
        return Ok(game);
    }

    /// <summary>
    /// 
    /// </summary>
    [HttpGet("Join")]
    public async Task Join([FromQuery] string UserName, [FromQuery] string Code)
    {
        logger.LogInformation("Received new connection");
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            logger.LogInformation("Connection is Websocket");
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            // Draw the rest of the fucking owl
        }
        else
        {
            logger.LogWarning("Connection is not a Websocket");
            HttpContext.Response.StatusCode = 400;
        }
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
}