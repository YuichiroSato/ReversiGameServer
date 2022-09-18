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
        public void EvolveBoardTest1()
        {
            string[,] borad = GameEngine.GetInitialBoard();
            Assert.That(borad[3, 3], Is.EqualTo("w"));
            GameEngine.EvolveBoard(borad, "b", 2, 3);
            Assert.That(borad[3, 3], Is.EqualTo("b"));
            GameEngine.EvolveBoard(borad, "w", 2, 2);
            Assert.That(borad[3, 3], Is.EqualTo("w"));
            GameEngine.EvolveBoard(borad, "b", 3, 2);
            Assert.That(borad[3, 3], Is.EqualTo("b"));
            GameEngine.EvolveBoard(borad, "w", 2, 4);
            Assert.That(borad[3, 2], Is.EqualTo("w"));
            Assert.That(borad[4, 3], Is.EqualTo("w"));
        }

       [Test]
        public void EvolveBoardTest2()
        {
            string[,] borad = new string[,]
            {
                { "b", "_", "b", "_", "_", "_", "_", "_" },
                { "_", "w", "w", "_", "_", "_", "_", "_" },
                { "b", "w", "_", "w", "w", "b", "w", "w" },
                { "_", "w", "w", "w", "b", "_", "_", "_" },
                { "b", "_", "w", "b", "w", "_", "_", "_" },
                { "_", "_", "b", "_", "_", "w", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "b", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "b", null, null, null, null, null, null, null }
            };
            string[,] expected = new string[,]
            {
                { "b", "_", "b", "_", "_", "_", "_", "_" },
                { "_", "b", "b", "_", "_", "_", "_", "_" },
                { "b", "b", "b", "b", "b", "b", "w", "w" },
                { "_", "b", "b", "b", "b", "_", "_", "_" },
                { "b", "_", "b", "b", "b", "_", "_", "_" },
                { "_", "_", "b", "_", "_", "b", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "b", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", null, null, null, null, null, null, null }
            };
            var result = GameEngine.EvolveBoard(borad, "b", 2, 2);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EvolveBoardTest3()
        {
            string[,] borad = new string[,]
            {
                { "b", "_", "b", "_", "_", "_", "_", "_" },
                { "_", "w", "w", "_", "_", "_", "_", "_" },
                { "b", "w", "_", "w", "w", "b", "w", "w" },
                { "_", "w", "w", "w", "b", "_", "_", "_" },
                { "b", "_", "w", "b", "w", "_", "_", "_" },
                { "_", "_", "b", "_", "_", "w", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "b", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "w", null, null, null, null, null, null, null }
            };
            string[,] expected = new string[,]
            {
                { "b", "_", "b", "_", "_", "_", "_", "_" },
                { "_", "w", "w", "_", "_", "_", "_", "_" },
                { "b", "w", "w", "w", "w", "b", "w", "w" },
                { "_", "w", "w", "w", "b", "_", "_", "_" },
                { "b", "_", "w", "b", "w", "_", "_", "_" },
                { "_", "_", "b", "_", "_", "w", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "b", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "b", null, null, null, null, null, null, null }
            };
            var result = GameEngine.EvolveBoard(borad, "w", 2, 2);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void IsGameEndTest1()
        {
            string[,] borad = GameEngine.GetInitialBoard();
            Assert.False(GameEngine.IsGameEnd(borad));
            borad[3, 3] = "_";
            borad[4, 3] = "_";
            borad[3, 4] = "_";
            borad[4, 4] = "_";
            Assert.True(GameEngine.IsGameEnd(borad));
        }

        [Test]
        public void IsGameEndTest2()
        {
            string[,] borad = new string[,]
            {
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "w", "w", "_", "_", "_" },
                { "_", "_", "_", "w", "w", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "w", null, null, null, null, null, null, null }
            };
            Assert.True(GameEngine.IsGameEnd(borad));
        }

        [Test]
        public void IsLegalMoveTest()
        {
            string[,] borad = new string[,]
            {
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "b", "w", "w", "_", "_", "b", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "w", "_", "_", "_" },
                { "_", "_", "_", "w", "_", "_", "w", "_" },
                { "_", "_", "b", "_", "_", "_", "w", "_" },
                { "_", "_", "_", "_", "_", "_", "b", "_" },
                { "b", null, null, null, null, null, null, null }
            };
            Assert.False(GameEngine.IsLegalMove(borad, "b", 0, 0));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 1, 0));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 2, 0));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 3, 0));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 4, 0));
            Assert.True(GameEngine.IsLegalMove(borad, "b", 4, 1));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 4, 2));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 3, 2));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 2, 2));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 1, 2));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 0, 2));

            Assert.False(GameEngine.IsLegalMove(borad, "b", 5, 0));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 6, 0));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 7, 0));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 7, 1));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 7, 2));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 6, 2));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 5, 2));

            Assert.True(GameEngine.IsLegalMove(borad, "b", 5, 3));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 5, 4));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 4, 5));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 3, 6));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 4, 3));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 3, 4));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 2, 5));

            Assert.True(GameEngine.IsLegalMove(borad, "b", 6, 4));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 7, 4));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 7, 5));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 7, 6));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 7, 7));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 5, 7));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 5, 6));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 5, 5));
            Assert.False(GameEngine.IsLegalMove(borad, "b", 5, 4));
        }
    }
}