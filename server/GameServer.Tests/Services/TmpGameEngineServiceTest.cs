using NUnit.Framework;
using GameServer.Services;

namespace Service.Tests
{
    public class Tests
    {
        private IGameEngineService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TmpGameEngineService();
        }

        [Test]
        public void GetBoardListTest()
        {
            _service.GetBoard(1);
            _service.GetBoard(2);
            var result = _service.GetBoardList();
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.ElementAt(0), Is.EqualTo("1"));
            Assert.That(result.ElementAt(1), Is.EqualTo("2"));
        }

        [Test]
        public void InitBoardTest()
        {
            _service.RemoveBoard(1);
            Assert.That(_service.InitBoard(1), Is.EqualTo(true));
            var board = _service.GetBoard(1);
            Assert.That(board, Is.EqualTo(GameEngine.GetInitialBoard()));
            //_service.RemoveBoard(1);
        }

        [Test]
        public void ReadBoardTest()
        {
            //var result = _service.GetBoard(2);
            //Assert.That(result, Is.EqualTo(null));
        }
    }
}