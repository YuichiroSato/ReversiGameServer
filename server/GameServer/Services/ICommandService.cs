using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Services
{
    public interface ICommandService
    {
        ReplyMessage Login(GameMessage message, CommunicationChannel channel);
        ReplyMessage Reload(GameMessage message, CommunicationChannel channel);
        ReplyMessage Move(GameMessage message, CommunicationChannel channel);
        ReplyMessage Push(CommunicationChannel channel);
    }

    public class ReplyMessage
    {
        public GameMessage gameMessage { get; set; }
        public int? playerNo { get; set; }
        public List<List<string>> board { get; set; }
        public byte[] buffer { get; set; }

        public static List<List<string>> toList(string[,] board)
        {
            var list = new List<List<string>>();
            for (int y = 0; y < 8; y++)
            {
                var line = new List<string>();
                for (int x = 0; x < 7; x++)
                {
                    line.Add(board[y, x]);
                }
                line.Add(board[y, 7]);
                list.Add(line);
            }
            list.Add(new List<string> { board[8, 0] });
            return list;
        }
    }
}