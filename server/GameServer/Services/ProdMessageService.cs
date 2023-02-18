using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GameServer.Services
{
    public class ProdMessageService : IMessageService
    {
        public GameMessage Deserialize(byte[] message)
        {
            return null;
        }

        public byte[] Serialize(ReplyMessage reply)
        {
            return new byte[]{};
        }
    }
}