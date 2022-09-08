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
            if (!IsLegalMove(borad, symbol, x, y))
            {
                throw new Exception($"This is not legal move x: {x} y: {y} player: {symbol}");
            }
            borad[y, x] = symbol;
            var endXY_1 = FindEndXY_1(borad, symbol, x, y);
            if (IsLegalIndex(endXY_1.x, endXY_1.y))
            {
                PutX(borad, symbol, x, endXY_1.x, y);
            }
            var endXY_2 = FindEndXY_2(borad, symbol, x, y);
            if (IsLegalIndex(endXY_2.x, endXY_2.y))
            {
                PutX(borad, symbol, endXY_2.x, x, y);
            }

            var endUD_1 = FindEndUD_1(borad, symbol, y, x);
            if (IsLegalIndex(endUD_1.x, endUD_1.y))
            {
                PutY(borad, symbol, y, endUD_1.y, x);
            }
            var endUD_2 = FindEndUD_2(borad, symbol, y, x);
            if (IsLegalIndex(endUD_2.x, endUD_2.y))
            {
                PutY(borad, symbol, endUD_2.y, y, x);
            }

            //todo dyagonl
            
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
            var endXY_1 = FindEndXY_1(borad, symbol, x, y);
            var endXY_2 = FindEndXY_2(borad, symbol, x, y);
            var endUD_1 = FindEndUD_1(borad, symbol, y, x);
            var endUD_2 = FindEndUD_2(borad, symbol, y, x);
            return IsLegalIndex(endXY_1.x, endXY_1.y)
                || IsLegalIndex(endXY_2.x, endXY_2.y)
                || IsLegalIndex(endUD_1.x, endUD_1.y)
                || IsLegalIndex(endUD_2.x, endUD_2.y);
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

        public static bool IsLegalIndex(int x, int y)
        {
            return x >= 0 && y >= 0 && x < 8 && y < 8;
        }

        public static (int x, int y) FindEndXY_1(string[,] borad, string symbol, int startX, int y)
        {
            string enemy = FlipSymbol(symbol);
            if (borad[y, startX] != symbol)
            {
                return (-1, -1);
            }
            for (int x = startX + 1; x < 8; x++)
            {
                if (borad[y, x] == enemy)
                {
                    continue;
                }
                else if (borad[y, x] == symbol)
                {
                    return (x, y);
                }
                else if (borad[y, x] == "_")
                {
                    return (-1, -1);
                }
            }
            return (-1, -1);
        }

        public static (int x, int y) FindEndXY_2(string[,] borad, string symbol, int startX, int y)
        {
            string enemy = FlipSymbol(symbol);
            if (borad[y, startX] != symbol)
            {
                return (-1, -1);
            }
            for (int x = startX - 1; x > -1; x--)
            {
                if (borad[y, x] == enemy)
                {
                    continue;
                }
                else if (borad[y, x] == symbol)
                {
                    return (x, y);
                }
                else if (borad[y, x] == "_")
                {
                    return (-1, -1);
                }
            }
            return (-1, -1);
        }

        public static (int x, int y) FindEndUD_1(string[,] borad, string symbol, int startY, int x)
        {
            string enemy = FlipSymbol(symbol);
            if (borad[startY, x] != symbol)
            {
                return (-1, -1);
            }
            for (int y = startY + 1; y < 8; y++)
            {
                if (borad[y, x] == enemy)
                {
                    continue;
                }
                else if (borad[y, x] == symbol)
                {
                    return (x, y);
                }
                else if (borad[y, x] == "_")
                {
                    return (-1, -1);
                }
            }
            return (-1, -1);
        }

        public static (int x, int y) FindEndUD_2(string[,] borad, string symbol, int startY, int x)
        {
            string enemy = FlipSymbol(symbol);
            if (borad[startY, x] != symbol)
            {
                return (-1, -1);
            }
            for (int y = startY - 1; y > -1; y--)
            {
                if (borad[y, x] == enemy)
                {
                    continue;
                }
                else if (borad[y, x] == symbol)
                {
                    return (x, y);
                }
                else if (borad[y, x] == "_")
                {
                    return (-1, -1);
                }
            }
            return (-1, -1);
        }

        public static string FlipSymbol(string symbol)
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

        public static string[,] PutX(string[,] board, string symbol, int startX, int endX, int y)
        {
            for (int x = startX; x < endX; x++)
            {
                board[y, x] = symbol;
            }
            return board;
        }

        public static string[,] PutY(string[,] board, string symbol, int startY, int endY, int x)
        {
            for (int y = startY; y < endY; y++)
            {
                board[y, x] = symbol;
            }
            return board;
        }

        public static string[,] PutRd(string[,] board, string symbol, int startX, int startY, int endX, int endY)
        {
            int x = startX;
            int y = startY;
            while (x < endX && y < endY)
            {
                board[x, y] = symbol;
                x++;
                y++;
            }
            return board;
        }

        public static string[,] PutLd(string[,] board, string symbol, int startX, int startY, int endX, int endY)
        {
            int x = startX;
            int y = startY;
            while (x > endX && y < endY)
            {
                board[x, y] = symbol;
                x--;
                y++;
            }
            return board;
        }
    }
}