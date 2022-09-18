using NUnit.Framework;
using GameServer.Services;

namespace Service.Tests
{
    public class TmpGameEngineServiceTests
    {
        private IGameEngineService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TmpGameEngineService();
            foreach(var f in _service.GetBoardList())
            {
                _service.RemoveBoard(Int32.Parse(f));
            }
        }

        [Test]
        public void GetBoardListTest()
        {
            _service.InitBoard(1);
            _service.InitBoard(2);
            var result = _service.GetBoardList();
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.ElementAt(0), Is.EqualTo("1"));
            Assert.That(result.ElementAt(1), Is.EqualTo("2"));
            _service.RemoveBoard(1);
            _service.RemoveBoard(2);
        }

        [Test]
        public void GetBoardTest()
        {
            _service.RemoveBoard(1);
            _service.InitBoard(1);
            var result = _service.GetBoard(1);
            Assert.That(result[8, 0], Is.EqualTo("b"));
            Assert.That(result[0, 0], Is.EqualTo("_"));
            Assert.That(result[3, 3], Is.EqualTo("w"));
            Assert.That(result[4, 3], Is.EqualTo("b"));
            _service.RemoveBoard(1);
        }

        [Test]
        public void InitBoardTest()
        {
            _service.RemoveBoard(1);
            Assert.That(_service.InitBoard(1), Is.EqualTo(true));
            var board = _service.GetBoard(1);
            Assert.That(board, Is.EqualTo(GameEngine.GetInitialBoard()));
            _service.RemoveBoard(1);
        }

        [Test]
        public void PlayMoveTest()
        {
            _service.RemoveBoard(1);
            _service.InitBoard(1);
            _service.PlayMove(1, "b", 3, 2);
            var result = _service.GetBoard(1);
            Assert.That(result[8, 0], Is.EqualTo("w"));
            Assert.That(result[2, 3], Is.EqualTo("b"));
            Assert.That(result[3, 3], Is.EqualTo("b"));
            Assert.That(result[4, 3], Is.EqualTo("b"));
            Assert.That(result[3, 4], Is.EqualTo("b"));
            Assert.That(result[4, 4], Is.EqualTo("w"));
            _service.RemoveBoard(1);
        }

        [Test]
        public void RemoveBoardTest()
        {
            _service.RemoveBoard(1);
            _service.InitBoard(1);
            var result = _service.GetBoard(1);
            Assert.That(result, Is.EqualTo(GameEngine.GetInitialBoard()));
            _service.RemoveBoard(1);
            try
            {
                _service.GetBoard(1);
                Assert.Fail();
            }
            catch
            {
                Assert.Pass();
            }
        }

        [Test]
        public void IsExistsTest()
        {
            _service.RemoveBoard(1);
            _service.InitBoard(1);
            Assert.True(_service.IsExists(1));
            _service.RemoveBoard(1);
            Assert.False(_service.IsExists(1));
        }

        [Test]
        public void IsGameEndTest()
        {
            _service.RemoveBoard(1);
            _service.InitBoard(1);
            Assert.False(_service.IsGameEnd(1));
            _service.RemoveBoard(1);
        }

        [Test]
        public void IsLegalMoveTest()
        {
            _service.RemoveBoard(1);
            _service.InitBoard(1);
            Assert.True(_service.IsLegalMove(1, "b", 3, 2));
            Assert.False(_service.IsLegalMove(1, "b", 0, 0));
            Assert.False(_service.IsLegalMove(1, "w", 3, 5));
            _service.RemoveBoard(1);
        }
    }
}