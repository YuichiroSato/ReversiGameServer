using NUnit.Framework;
using GameServer.Services;

namespace Service.Tests
{
    public class GameEngineTests
    {
        [SetUp]
        public void Setup()
        {
            ;
        }

        [Test]
        public void GetInitialBoradTest()
        {
            var result = GameEngine.GetInitialBoard();
            Assert.That(result, Is.Not.EqualTo(null));
            Assert.That(result.GetLength(0), Is.EqualTo(9));
            Assert.That(result.GetLength(1), Is.EqualTo(8));
            Assert.That(result[3, 3], Is.EqualTo("w"));
            Assert.That(result[3, 4], Is.EqualTo("b"));
            Assert.That(result[7, 7], Is.EqualTo("_"));
            Assert.That(result[8, 0], Is.EqualTo("b"));
        }

        [Test]
        public void ParseLineTest()
        {
            var result = GameEngine.ParseLine("_ _ w b");
            Assert.That(result, Is.EqualTo(new string[] { "_", "_", "w", "b" }));
        }

        [Test]
        public void EvolveBoardTest()
        {
            string[,] borad = GameEngine.GetInitialBoard();
            Assert.That(borad[3, 3], Is.EqualTo("w"));
            GameEngine.PrintBoard(borad);
            GameEngine.EvolveBoard(borad, "b", 2, 3);
            GameEngine.PrintBoard(borad);
            Assert.That(borad[3, 3], Is.EqualTo("b"));
        }

        [Test]
        public void IsGameEndTest()
        {
            string[,] borad = GameEngine.GetInitialBoard();
            Assert.That(GameEngine.IsGameEnd(borad), Is.EqualTo(false));
            borad[3, 3] = "_";
            borad[4, 3] = "_";
            borad[3, 4] = "_";
            borad[4, 4] = "_";
            Assert.That(GameEngine.IsGameEnd(borad), Is.EqualTo(true));
        }
    }
}