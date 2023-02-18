using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using GameServer.Services;

namespace GameServer.Controllers
{
    [Route("api/ws")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly IGameEngineService _gameEngineService;
        private readonly IMessageService _DevMessageService;
        private readonly IMessageService _ProdMessageService;
        private readonly ICommandService _CommandService;
        private readonly DevMessageService _MessageService;

        public WebSocketController(IGameEngineService gameEngineService)
        {
            _gameEngineService = gameEngineService;
            _DevMessageService = new DevMessageService();
            _ProdMessageService = new ProdMessageService();
            _CommandService = new CommandService(gameEngineService);
            _MessageService = new DevMessageService();
        }

        [HttpGet("connect/{id}")]
        public async Task Prod(int id)
        {
            await Worker(id, _ProdMessageService);
        }

        [HttpGet("connect/dev/{id}")]
        public async Task Dev(int id)
        {
            await Worker(id, _DevMessageService);
        }

        private async Task Worker(int id, IMessageService messageService)
        {
            try
            {
                await Task.Run(async () =>
                {
                    var channel = new CommunicationChannel();
                    channel.RoomNo = id;
                    if (HttpContext.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                        Task.Run(async () => pushWorker(webSocket, channel, messageService));
                        while(webSocket.State == WebSocketState.Connecting
                            || webSocket.State == WebSocketState.Open)
                        {
                            var buffer = new byte[1024 * 4];
                            var receiveResult = await webSocket.ReceiveAsync(
                                new ArraySegment<byte>(buffer),
                                CancellationToken.None);
                            var message = messageService.Deserialize(buffer);
                            var reply = new ReplyMessage();
                            switch(message.command)
                            {
                                case "login":
                                   reply = _CommandService.Login(message, channel);
                                break;
                                case "reload":
                                    reply = _CommandService.Reload(message, channel);
                                break;
                                case "move":
                                    reply = _CommandService.Move(message, channel);
                                break;
                            }
                            buffer = messageService.Serialize(reply);
                            await webSocket.SendAsync(
                                new ArraySegment<byte>(buffer, 0, buffer.Length),
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
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private async void pushWorker(WebSocket webSocket, CommunicationChannel channel, IMessageService messageService)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                while(webSocket.State == WebSocketState.Connecting
                    || webSocket.State == WebSocketState.Open)
                {
                    if (channel.PlayerNo == 3)
                    {
                        Thread.Sleep(200);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                    var reply = _CommandService.Push(channel);
                    if (reply.board != null)
                    {
                        var buffer = messageService.Serialize(reply);
                        await webSocket.SendAsync(
                            new ArraySegment<byte>(buffer, 0, buffer.Length),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
