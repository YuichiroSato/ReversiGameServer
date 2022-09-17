using NUnit.Framework;
using GameServer.Services;

namespace Service.Tests
{
    public class PieceEnumeratorTests
    {
        [SetUp]
        public void Setup()
        {
            ;
        }

        [Test]
        public void EnumeratorTest1()
        {
            var initBoard = GameEngine.GetInitialBoard();
            var generator1 = new PiecesGenerator(initBoard, 0, 0);
            var pieces1 = generator1.GetPieces(PiecesType.Type_1);
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces1.MoveNext());

            var generator2 = new PiecesGenerator(initBoard, 4, 4);
            var pieces2 = generator2.GetPieces(PiecesType.Type_1);
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("b"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces2.MoveNext());
        }

        [Test]
        public void EnumeratorTest2()
        {
            var initBoard = GameEngine.GetInitialBoard();
            var generator1 = new PiecesGenerator(initBoard, 0, 0);
            var pieces1 = generator1.GetPieces(PiecesType.Type_2);
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces1.MoveNext());

            var generator2 = new PiecesGenerator(initBoard, 4, 4);
            var pieces2 = generator2.GetPieces(PiecesType.Type_2);
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces2.MoveNext());
        }

        [Test]
        public void EnumeratorTest3()
        {
            var initBoard = GameEngine.GetInitialBoard();
            var generator1 = new PiecesGenerator(initBoard, 0, 0);
            var pieces1 = generator1.GetPieces(PiecesType.Type_3);
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces1.MoveNext());

            var generator2 = new PiecesGenerator(initBoard, 4, 4);
            var pieces2 = generator2.GetPieces(PiecesType.Type_3);
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces2.MoveNext());
        }

        [Test]
        public void EnumeratorTest4()
        {
            var initBoard = GameEngine.GetInitialBoard();
            var generator1 = new PiecesGenerator(initBoard, 0, 0);
            var pieces1 = generator1.GetPieces(PiecesType.Type_4);
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces1.MoveNext());

            var generator2 = new PiecesGenerator(initBoard, 4, 4);
            var pieces2 = generator2.GetPieces(PiecesType.Type_4);
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces2.MoveNext());
        }

        [Test]
        public void EnumeratorTest5()
        {
            var initBoard = GameEngine.GetInitialBoard();
            var generator1 = new PiecesGenerator(initBoard, 0, 0);
            var pieces1 = generator1.GetPieces(PiecesType.Type_5);
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces1.MoveNext());
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces1.MoveNext());

            var generator2 = new PiecesGenerator(initBoard, 4, 4);
            var pieces2 = generator2.GetPieces(PiecesType.Type_5);
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces2.MoveNext());
        }

        [Test]
        public void EnumeratorTest6()
        {
            var initBoard = GameEngine.GetInitialBoard();
            var generator1 = new PiecesGenerator(initBoard, 0, 0);
            var pieces1 = generator1.GetPieces(PiecesType.Type_6);
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces1.MoveNext());

            var generator2 = new PiecesGenerator(initBoard, 4, 4);
            var pieces2 = generator2.GetPieces(PiecesType.Type_6);
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces2.MoveNext());
        }

        [Test]
        public void EnumeratorTest7()
        {
            var initBoard = GameEngine.GetInitialBoard();
            var generator1 = new PiecesGenerator(initBoard, 0, 0);
            var pieces1 = generator1.GetPieces(PiecesType.Type_7);
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces1.MoveNext());

            var generator2 = new PiecesGenerator(initBoard, 4, 4);
            var pieces2 = generator2.GetPieces(PiecesType.Type_7);
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("b"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces2.MoveNext());
        }

        [Test]
        public void EnumeratorTest8()
        {
            var initBoard = GameEngine.GetInitialBoard();
            var generator1 = new PiecesGenerator(initBoard, 0, 0);
            var pieces1 = generator1.GetPieces(PiecesType.Type_8);
            Assert.That(pieces1.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces1.MoveNext());

            var generator2 = new PiecesGenerator(initBoard, 4, 4);
            var pieces2 = generator2.GetPieces(PiecesType.Type_8);
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("w"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsTrue(pieces2.MoveNext());
            Assert.That(pieces2.Current, Is.EqualTo("_"));
            Assert.IsFalse(pieces2.MoveNext());
        }
    }
}