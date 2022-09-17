using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Services
{
    public class GameEngine
    {
        public static string[,] GetInitialBoard()
        {
            return new string[,]
            {
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "w", "b", "_", "_", "_" },
                { "_", "_", "_", "b", "w", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "_", "_", "_", "_", "_", "_", "_", "_" },
                { "b", null, null, null, null, null, null, null }
            };
        }

        public static void PrintBoard(string[,] board)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Console.Write(board[y, x] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine(board[8, 0]);
        }

        public static string[] ParseLine(string line)
        {
            return line.Split(null);
        }

        public static string[,] EvolveBoard(string[,] borad, string symbol, int x,int y)
        {
            borad[y, x] = symbol;
            foreach (var pieces in (new PiecesGenerator(borad, x, y)).GetAllPieces())
            {
                var xy = FindPair(pieces, symbol);
                if (IsLegalIndex(xy.x, xy.y))
                {
                    borad = PutPieces(borad, symbol, xy.x, xy.y, pieces);
                }
            }
            borad[8, 0] = FlipSymbol(symbol);
            if (IsGameEnd(borad))
            {
                borad[8, 0] = "_";
            }
            return borad;
        }

        public static bool IsLegalMove(string[,] _borad, string symbol, int x, int y)
        {
            // check player symbol
            if (symbol != _borad[8, 0])
            {
                return false;
            }
            string[,] borad = (string[,])_borad.Clone();
            borad[y, x] = symbol;
            foreach (var pieces in (new PiecesGenerator(borad, x, y)).GetAllPieces())
            {
                var xy = FindPair(pieces, symbol);
                if (IsLegalIndex(xy.x, xy.y))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsGameEnd(string[,] _borad)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (_borad[y, x] == "_"
                        && (IsLegalMove(_borad, "b", x, y)
                        || IsLegalMove(_borad, "w", x, y)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool IsLegalIndex(int x, int y)
        {
            return x >= 0 && y >= 0 && x < 8 && y < 8;
        }

        private static (int x, int y) FindPair(PieceEnum pieces, string symbol)
        {
            string enemy = FlipSymbol(symbol);
            if (pieces.Current != symbol)
            {
                return (-1, -1);
            }
            while (pieces.IsLegalIndex && pieces.MoveNext())
            {
                if (pieces.Current == enemy)
                {
                    continue;
                }
                else if (pieces.Current == symbol)
                {
                    return pieces.CurrentPosition;
                }
                else if (pieces.Current == "_")
                {
                    return (-1, -1);
                }
            }
            return (-1, -1);
        }

        private static string FlipSymbol(string symbol)
        {
            if (symbol == "w")
            {
                return "b";
            }
            else if (symbol == "b")
            {
                return "w";
            }
            else
            {
                throw new Exception($"This is not legal symbol: {symbol}");
            }
        }

        private static string[,] PutPieces(string[,] board, string symbol, int endX, int endY, PieceEnum pieces)
        {
            pieces.Reset();
            while (pieces.IsLegalIndex
                && pieces.MoveNext()
                && (pieces.CurrentPosition.x != endX || pieces.CurrentPosition.y != endY))
            {
                board[pieces.CurrentPosition.y, pieces.CurrentPosition.x] = symbol;
            }
            return board;
        }
    }
}