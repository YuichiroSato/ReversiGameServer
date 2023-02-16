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
                            var message = messageService.parse(buffer);
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
                            if (reply.buffer != null)
                            {
                                await webSocket.SendAsync(
                                    new ArraySegment<byte>(reply.buffer, 0, reply.buffer.Length),
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
                    Thread.Sleep(1000);
                    var reply = _CommandService.Push(channel);
                    if (reply.buffer != null)
                    {
                        await webSocket.SendAsync(
                            new ArraySegment<byte>(reply.buffer, 0, reply.buffer.Length),
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

        [HttpGet("connect")]
        public async Task Get()
        {
            try {
            await Task.Run(async () =>
            {
                var roomNo = new int[] { -1, -1 };
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    try{ Task.Run(async () => push(webSocket, roomNo)); } catch (Exception e) {;}
                    while(webSocket.State == WebSocketState.Connecting
                        || webSocket.State == WebSocketState.Open)
                    {
                        var buffer = new byte[1024 * 4];
                        var receiveResult = await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer), CancellationToken.None);
                        var message = _MessageService.deserialize(buffer);
                        if (message.command == "login")
                        {
                            var matched = false;
                            var text = "";
                            var rooms = _gameEngineService.GetBoardList();
                            foreach(var room in rooms)
                            {
                                var state = _gameEngineService.GetGameState(Int32.Parse(room));
                                if (state == GameState.Matching)
                                {
                                    roomNo[0] = Int32.Parse(room);
                                    roomNo[1] = 2;
                                    matched = true;
                                    _gameEngineService.SetGameState(Int32.Parse(room), GameState.Bplaying);
                                    text = "login-as:w";
                                }
                            }
                            if (!matched)
                            {
                                //ToDo fix conflict
                                _gameEngineService.InitBoard(rooms.Count + 1);
                                _gameEngineService.SetGameState(rooms.Count + 1, GameState.Matching);
                                roomNo[0] = rooms.Count + 1;
                                roomNo[1] = 1;
                                text = "login-as:b";
                            }
                            buffer = System.Text.Encoding.UTF8.GetBytes(text);
                        }
                        else if (message.command == "reload")
                        {
                            var str = boardString(roomNo[0]);
                            buffer = System.Text.Encoding.UTF8.GetBytes(str);
                        }
                        else if (message.command == "move")
                        {
                            var symbol = message.symbol ?? "";
                            var x = message.x == null ? -1 : Int32.Parse(message.x ?? "");
                            var y = message.y == null ? -1 : Int32.Parse(message.y ?? "");
                            if (symbol != "" && x != -1 && y != -1
                                && _gameEngineService.IsLegalMove(roomNo[0], symbol, x, y))
                            {
                                _gameEngineService.PlayMove(roomNo[0], symbol, x, y);
                                if (roomNo[1] == 1)
                                {
                                    _gameEngineService.SetGameState(roomNo[0], GameState.Wpushing);
                                }
                                else
                                {
                                    _gameEngineService.SetGameState(roomNo[0], GameState.Bpushing);
                                }
                                var str = boardString(roomNo[0]);
                                buffer = System.Text.Encoding.UTF8.GetBytes(str);
                            }
                        }
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
            } catch (Exception e) {}
        }

        private async void push(WebSocket webSocket, int[] roomNo)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {        
                while(webSocket.State == WebSocketState.Connecting
                    || webSocket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    Thread.Sleep(1000);
                    if (roomNo[0] != -1 && roomNo[1] != -1)
                    {
                        var state = _gameEngineService.GetGameState(roomNo[0]);
                        if ((roomNo[1] == 1 && state == GameState.Bpushing)
                            || roomNo[1] == 2 && state == GameState.Wpushing)
                        {
                            var str = boardString(roomNo[0]);
                            buffer = System.Text.Encoding.UTF8.GetBytes(str);
                            await webSocket.SendAsync(
                                new ArraySegment<byte>(buffer, 0, buffer.Length),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);
                            if (state == GameState.Bpushing)
                            {
                                _gameEngineService.SetGameState(roomNo[0], GameState.Wplayng);
                            }
                            else if (state == GameState.Wpushing)
                            {
                                _gameEngineService.SetGameState(roomNo[0], GameState.Bplaying);
                            }
                        }
                    }
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private String boardString(int id)
        {
            var board = _gameEngineService.GetBoard(id);
            var str = "";
            for (int y = 0; y < 8; y++)
            {
                var line = "";
                for (int x = 0; x < 7; x++)
                {
                    line += board[y, x] + ","; 
                }
                str += line + board[y, 7] + "\n";
            }
            str += board[8, 0] + "\n";
            return str;
        }
    }
}
