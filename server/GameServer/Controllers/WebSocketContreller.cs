using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace GameServer.Controllers
{
    [Route("api/ws")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        public WebSocketController()
        {
            ;
        }

        [HttpGet("connect")]
        public async Task Get()
        {
            await Task.Run(async () =>
            {
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    while(webSocket.State == WebSocketState.Connecting
                        || webSocket.State == WebSocketState.Open)
                    {
                        var buffer = new byte[1024 * 4];
                        var receiveResult = await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer), CancellationToken.None);
                        await webSocket.SendAsync(
                            new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }
                }
                else
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            });
        }
    }
}
