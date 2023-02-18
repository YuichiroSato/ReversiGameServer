using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GameServer.Services
{
    public class DevMessageService : IMessageService
    {
        public GameMessage Deserialize(byte[] message)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(TrimByte(message));
            return JsonSerializer.Deserialize<GameMessage>(jsonString);
        }

        public byte[] Serialize(ReplyMessage reply)
        {
            var str = JsonSerializer.Serialize(reply);
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        private byte[] TrimByte(byte[] message)
        {
            var c = 0;
            for (int i = 0; i < message.Length; i++)
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
}