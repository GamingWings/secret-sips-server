using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SecretSipsServer.Controllers;

[ApiController]
[Route("[controller]")]
public class SecretSipsController : ControllerBase, IActionFilter
{
    private readonly ILogger<SecretSipsController> _logger;

    public SecretSipsController(ILogger<SecretSipsController> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            context.Result = BadRequest(); // Sets the response status code to 400 and returns immediately
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }


    [HttpGet("Join")]
    public async Task Join()
    {
        Console.WriteLine("Received new connection");
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            Console.WriteLine("Connection is Websocket");
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[1024 * 4];
            var message = Encoding.UTF8.GetBytes("Hello World");
            await webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        else
        {
            Console.WriteLine("Connection is not Websocket");
            HttpContext.Response.StatusCode = 400;
        }
    }

    [HttpGet("Create")]
    public async Task Create()
    {
        Console.WriteLine("Received new connection");
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            Console.WriteLine("Connection is Websocket");
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[1024 * 4];
            var message = Encoding.UTF8.GetBytes("Hello World");
            await webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        else
        {
            Console.WriteLine("Connection is not Websocket");
            HttpContext.Response.StatusCode = 400;
        }
    }
}