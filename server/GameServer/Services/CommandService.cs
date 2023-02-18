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
            }
            else
            {
                var state = _gameEngineService.GetGameState(roomNo);
                if (state == GameState.Matching)
                {
                    channel.PlayerNo = 2;
                    _gameEngineService.SetGameState(roomNo, GameState.Bplaying);
                }
                else
                {
                    channel.PlayerNo = 3;
                }
            }
            var reply = new ReplyMessage();
            reply.gameMessage = message;
            reply.board = ReplyMessage.toList(_gameEngineService.GetBoard(channel.RoomNo ?? -1));
            reply.playerNo = channel.PlayerNo;
            return reply;
        }

        public ReplyMessage Reload(GameMessage message, CommunicationChannel channel)
        {
            var reply = new ReplyMessage();
            reply.board =  ReplyMessage.toList(_gameEngineService.GetBoard(channel.RoomNo ?? -1));
            reply.gameMessage = message;
            return reply;
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
            }
            reply.gameMessage = message;
            reply.board = ReplyMessage.toList(_gameEngineService.GetBoard(channel.RoomNo ?? -1));
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
                    || playerNo == 2 && state == GameState.Wpushing
                    || playerNo == 3 && state == GameState.Bpushing
                    || playerNo == 3 && state == GameState.Wpushing)
                {
                    reply.board = ReplyMessage.toList(_gameEngineService.GetBoard(channel.RoomNo ?? -1));
                    if (state == GameState.Bpushing && playerNo == 1)
                    {
                        _gameEngineService.SetGameState(roomNo, GameState.Wplayng);
                    }
                    else if (state == GameState.Wpushing && playerNo == 2)
                    {
                        _gameEngineService.SetGameState(roomNo, GameState.Bplaying);
                    }
                }
            } 
            return reply;
        }
    }
}