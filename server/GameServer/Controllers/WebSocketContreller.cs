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
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[] { (byte)'a', (byte)'b', (byte)'c' };
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, 3),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
