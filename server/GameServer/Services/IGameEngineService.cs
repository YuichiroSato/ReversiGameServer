using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Services
{
    public interface IGameEngineService
    {
        List<int> GetBoardList();
        string[,] GetBoard(int id);
        bool InitBoard(int id);
        bool PlayMove(int id, string m, int x, int y);
        bool RemoveBoard(int id);
        bool IsExists(int id);
        bool IsGameEnd(int id);
        bool IsLegalMove(int id, string m, int x, int y);
    }
}