using NUnit.Framework;
using GameServer.Services;

namespace Service.Tests
{
    public class CommandServiceTests
    {
        private IGameEngineService _GameEngineService;
        private ICommandService _CommandService;
        private IMessageService _MessageService;
        [SetUp]
        public void Setup()
        {
            _GameEngineService = new TmpGameEngineService();
            foreach(var f in _GameEngineService.GetBoardList())
            {
                _GameEngineService.RemoveBoard(Int32.Parse(f));
            }
            _CommandService = new CommandService(_GameEngineService);
            _MessageService = new DevMessageService();
        }

        [Test]
        public void LoginTest()
        {
            var bytes = new byte[1024 * 4];
            bytes = EncodeBytes("{ \"command\": \"login\", \"loginName\": \"aaa\" }");
            var input = _MessageService.Deserialize(bytes);
            var channel = new CommunicationChannel();
            channel.RoomNo = 1;
            
            var result = _CommandService.Login(input, channel);
            Assert.That(result.gameMessage.command, Is.EqualTo("login"));
            Assert.That(result.gameMessage.loginName, Is.EqualTo("aaa"));
            Assert.That(result.playerNo, Is.EqualTo(1));

            bytes = new byte[1024 * 4];
            bytes = EncodeBytes("{ \"command\": \"login\", \"loginName\": \"bbb\" }");
            input = _MessageService.Deserialize(bytes);

            var result2 = _CommandService.Login(input, channel);
            Assert.That(result2.gameMessage.command, Is.EqualTo("login"));
            Assert.That(result2.gameMessage.loginName, Is.EqualTo("bbb"));
            Assert.That(result2.playerNo, Is.EqualTo(2));

            bytes = new byte[1024 * 4];
            bytes = EncodeBytes("{ \"command\": \"login\", \"loginName\": \"ccc\" }");
            input = _MessageService.Deserialize(bytes);

            var result3 = _CommandService.Login(input, channel);
            Assert.That(result3.gameMessage.command, Is.EqualTo("login"));
            Assert.That(result3.gameMessage.loginName, Is.EqualTo("ccc"));
            Assert.That(result3.playerNo, Is.EqualTo(3));
        }

        [Test]
        public void ReloadTest()
        {
            var bytes = new byte[1024 * 4];
            bytes = EncodeBytes("{ \"command\": \"reload\" }");
            var input = _MessageService.Deserialize(bytes);
            var channel = new CommunicationChannel();
            channel.RoomNo = 2;
            _GameEngineService.InitBoard(2);

            var result = _CommandService.Reload(input, channel);
            Assert.That(result.gameMessage.command, Is.EqualTo("reload"));
            Assert.That(result.board[0][0], Is.EqualTo("_"));
            Assert.That(result.board[3][3], Is.EqualTo("w"));
            Assert.That(result.board[3][4], Is.EqualTo("b"));
            Assert.That(result.board[4][3], Is.EqualTo("b"));
            Assert.That(result.board[4][4], Is.EqualTo("w"));
        }

        [Test]
        public void MoveTest()
        {
            var bytes = new byte[1024 * 4];
            bytes = EncodeBytes("{ \"command\": \"move\", \"symbol\": \"b\", \"x\": \"3\", \"y\": \"2\" }");
            var input = _MessageService.Deserialize(bytes);
            var channel = new CommunicationChannel();
            channel.RoomNo = 3;
            _GameEngineService.InitBoard(3);

            var result = _CommandService.Move(input, channel);
            Assert.That(result.gameMessage.command, Is.EqualTo("move"));
            Assert.That(result.board[0][0], Is.EqualTo("_"));
            Assert.That(result.board[2][3], Is.EqualTo("b"));
            Assert.That(result.board[3][3], Is.EqualTo("b"));
            Assert.That(result.board[3][4], Is.EqualTo("b"));
            Assert.That(result.board[4][3], Is.EqualTo("b"));
            Assert.That(result.board[4][4], Is.EqualTo("w"));
        }

        [Test]
        public void PushTest()
        {
            var channel = new CommunicationChannel();
            channel.RoomNo = 4;
            channel.PlayerNo = 1;
            _GameEngineService.InitBoard(4);
            _GameEngineService.SetGameState(4, GameState.Bpushing);

            var result = _CommandService.Push(channel);
            Assert.IsNull(result.gameMessage);
            Assert.That(result.board[0][0], Is.EqualTo("_"));
            Assert.That(result.board[3][3], Is.EqualTo("w"));
            Assert.That(result.board[3][4], Is.EqualTo("b"));
            Assert.That(result.board[4][3], Is.EqualTo("b"));
            Assert.That(result.board[4][4], Is.EqualTo("w"));

            channel.PlayerNo = 2;
            _GameEngineService.SetGameState(4, GameState.Bpushing);

            var result2 = _CommandService.Push(channel);
            Assert.IsNull(result2.gameMessage);
            Assert.IsNull(result2.board);

            _GameEngineService.SetGameState(4, GameState.Wpushing);

            var result3 = _CommandService.Push(channel);
            Assert.IsNull(result3.gameMessage);
            Assert.That(result3.board[0][0], Is.EqualTo("_"));
            Assert.That(result3.board[3][3], Is.EqualTo("w"));
            Assert.That(result3.board[3][4], Is.EqualTo("b"));
            Assert.That(result3.board[4][3], Is.EqualTo("b"));
            Assert.That(result3.board[4][4], Is.EqualTo("w"));

            channel.PlayerNo = 1;

            var result4 = _CommandService.Push(channel);
            Assert.IsNull(result4.gameMessage);
            Assert.IsNull(result4.board);
        }

        private byte[] EncodeBytes(string str)
        {
            var buffer = new byte[1024 * 4];
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[i] = bytes[i];
            }
            return buffer;
        }
    }
}