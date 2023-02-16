using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Services
{
    public interface IMessageService
    {
        GameMessage parse(byte[] message);
        byte[] encode(ReplyMessage reply);
    }

    public class GameMessage
    {
        public string? command { get; set; }
        public string? loginName { get; set; }
        public string? x { get; set; }
        public string? y { get; set; }
        public string? symbol { get; set; }
    }

    public class CommunicationChannel
    {
        public int? RoomNo { get; set; }
        public int? PlayerNo { get; set; }
    }
}