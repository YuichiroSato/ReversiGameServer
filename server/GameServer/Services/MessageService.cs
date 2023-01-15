using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GameServer.Services
{
    public class MessageService
    {
        public GameMessage deserialize(byte[] message)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(trimByte(message));
            return JsonSerializer.Deserialize<GameMessage>(jsonString);
        }

        private byte[] trimByte(byte[] message)
        {
            var c = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (message[i] != 0x00)
                {
                    c++;
                }
            }
            var trimed = new byte[c];
            for (int i = 0; i < c; i++)
            {
                trimed[i] = message[i];
            }
            return trimed;
        }
    }

    public class GameMessage
    {
        public string? command { get; set; }
        public string? loginName { get; set; }
        public string? x { get; set; }
        public string? y { get; set; }
        public string? symbol { get; set; }
    }
}