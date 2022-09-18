using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameServer.Services;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IGameEngineService _gameEngineService;

        public RoomsController(IGameEngineService gameEngineService)
        {
            _gameEngineService = gameEngineService;
        }
        
        [HttpGet()]
        public ActionResult<IEnumerable<string>> Get()
        {
            return _gameEngineService.GetBoardList();
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (!_gameEngineService.IsExists(id))
            {
                _gameEngineService.InitBoard(id);
            }
            var board = _gameEngineService.GetBoard(id);
            var str = "";
            for (int y= 0; y < 8; y++)
            {
                var line = "";
                for (int x = 0; x < 7; x++)
                {
                    line += board[y, x] + ","; 
                }
                str += line + board[y, 7] + "\n";
            }
            str += board[8, 0] + "\n";
            return str;
        }

        [HttpPost("{id}")]
        public string Post(int id)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string plainText = reader.ReadToEnd();
                string[] move = plainText.Split(null);
                string symbol = move[0];
                int x = Int32.Parse(move[1]);
                int y = Int32.Parse(move[2]);
                 _gameEngineService.PlayMove(id, symbol, x, y);
            }
            return "ok";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            _gameEngineService.RemoveBoard(id);
            return "ok";
        }
    }
}
