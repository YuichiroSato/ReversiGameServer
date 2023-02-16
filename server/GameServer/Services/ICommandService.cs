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
        public byte[] buffer { get; set; }
    }
}