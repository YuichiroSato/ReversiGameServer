using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Services
{
    public class CommandService : ICommandService
    {
        private readonly IGameEngineService _gameEngineService;
        public CommandService(IGameEngineService gameEngineService)
        {
            _gameEngineService = gameEngineService;
        }

        public ReplyMessage Login(GameMessage message, CommunicationChannel channel)
        {
            var roomNo = channel.RoomNo ?? -1;
            var text = "";
            if (!_gameEngineService.IsExists(roomNo))
            {
                _gameEngineService.InitBoard(roomNo);
                _gameEngineService.SetGameState(roomNo, GameState.Matching);
                channel.PlayerNo = 1;
                text = "login-as:b";
            }
            else
            {
                var state = _gameEngineService.GetGameState(roomNo);
                if (state == GameState.Matching)
                {
                    channel.PlayerNo = 2;
                    _gameEngineService.SetGameState(roomNo, GameState.Bplaying);
                    text = "login-as:w";
                }
            }
            var reply = new ReplyMessage();
            reply.buffer = System.Text.Encoding.UTF8.GetBytes(text);
            return reply;
        }

        public ReplyMessage Reload(GameMessage message, CommunicationChannel channel)
        {
            var reply = new ReplyMessage();
            var str = boardString(channel.RoomNo ?? -1);
            reply.buffer = System.Text.Encoding.UTF8.GetBytes(str);
            return reply;
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
        public ReplyMessage Move(GameMessage message, CommunicationChannel channel)
        {
            var roomNo = channel.RoomNo ?? -1;
            var playerNo = channel.PlayerNo ?? -1;
            var reply = new ReplyMessage();
            var symbol = message.symbol ?? "";
            var x = message.x == null ? -1 : Int32.Parse(message.x ?? "");
            var y = message.y == null ? -1 : Int32.Parse(message.y ?? "");
            if (symbol != "" && x != -1 && y != -1
                && _gameEngineService.IsLegalMove(roomNo, symbol, x, y))
            {
                _gameEngineService.PlayMove(roomNo, symbol, x, y);
                if (playerNo == 1)
                {
                    _gameEngineService.SetGameState(roomNo, GameState.Wpushing);
                }
                else
                {
                    _gameEngineService.SetGameState(roomNo, GameState.Bpushing);
                }
                var str = boardString(roomNo);
                reply.buffer = System.Text.Encoding.UTF8.GetBytes(str);
            }
            return reply;
        }

        public ReplyMessage Push(CommunicationChannel channel)
        {
            var roomNo = channel.RoomNo ?? -1;
            var playerNo = channel.PlayerNo ?? -1;
            var reply = new ReplyMessage();
            if (roomNo != -1 && playerNo != -1)
            {
                var state = _gameEngineService.GetGameState(roomNo);
                if (playerNo == 1 && state == GameState.Bpushing
                    || playerNo == 2 && state == GameState.Wpushing)
                {
                    var str = boardString(roomNo);
                    reply.buffer = System.Text.Encoding.UTF8.GetBytes(str);
                    if (state == GameState.Bpushing)
                    {
                        _gameEngineService.SetGameState(roomNo, GameState.Wplayng);
                    }
                    else if (state == GameState.Wpushing)
                    {
                        _gameEngineService.SetGameState(roomNo, GameState.Bplaying);
                    }
                }
            } 
            return reply;
        }
    }
}