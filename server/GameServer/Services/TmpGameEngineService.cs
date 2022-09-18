using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Services
{
    public class TmpGameEngineService : IGameEngineService
    {
        private static string RootPath = "/tmp/reversi/";

        public List<string> GetBoardList()
        {
            var ls = new List<string>();
            foreach (string f in Directory.GetFiles(RootPath))
            {
                ls.Add(Path.GetFileName(f));
            }
            return ls;
        }

        public string[,] GetBoard(int i)
        {
            return ReadBoardFile(i);
        }
        
        public bool InitBoard(int i)
        {
            if (!IsFileExists(i))
            {
                try
                {
                    CreateBoardFile(i);
                    WriteBoardFile(i, GameEngine.GetInitialBoard());
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                    return false;
                }
            }
            return true;
        }

        public bool PlayMove(int i, string m, int x, int y)
        {
            if (IsExists(i))
            {
                var borad = ReadBoardFile(i);
                borad = GameEngine.EvolveBoard(borad, m, x, y);
                WriteBoardFile(i, borad);
                return true;
            }
            return false;
        }

        public bool RemoveBoard(int i)
        {
            try
            {
                RemoveBoardFile(i);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return false;
            }
            return true;
        }

        public bool IsExists(int i)
        {
            return IsFileExists(i);
        }

        public bool IsGameEnd(int i)
        {
            if (IsExists(i))
            {
                var borad = ReadBoardFile(i);
                return GameEngine.IsGameEnd(borad);
            }
            return false;
        }

        public bool IsLegalMove(int i, string m, int x, int y)
        {
            if (IsExists(i))
            {
                var borad = ReadBoardFile(i);
                return GameEngine.IsLegalMove(borad, m, x, y);
            }
            return false;
        }

        private bool IsFileExists(int i)
        {
            return System.IO.File.Exists(RootPath + i);
        }

        private void CreateBoardFile(int i)
        {
            CreateRootDirectory();
            using (System.IO.FileStream fs = System.IO.File.Create(RootPath + i))
            {
                ;
            }
        }

        private void RemoveBoardFile(int i)
        {
            System.IO.File.Delete(RootPath + i);
        }

        private void CreateRootDirectory()
        {
            if (!System.IO.File.Exists(RootPath))
            {
                System.IO.Directory.CreateDirectory(RootPath);
            }
        }

        private string[,] ReadBoardFile(int id)
        {
            string[,] result = new string[9, 8];
            int i = 0;
            foreach (string line in System.IO.File.ReadLines(RootPath + id))
            {
                string[] symbols = GameEngine.ParseLine(line);
                for (int j = 0; j < symbols.GetLength(0); j++)
                {
                    result[i, j] = symbols[j];
                }
                i++;
            }
            return result;
        }

        private void WriteBoardFile(int id, string[,] board)
        {
            string[] strList = new string[9];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                string str = "";
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != null)
                    {
                        str += board[i, j] + " ";
                    }
                }
                str = str.Trim();
                strList[i] = str;
            }
            System.IO.File.WriteAllLines(RootPath + id, strList);
        }
    }
}