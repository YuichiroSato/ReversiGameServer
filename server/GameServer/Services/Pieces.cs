using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Services
{
    public enum PiecesType { Type_1, Type_2, Type_3, Type_4, Type_5, Type_6, Type_7, Type_8 };

    public class PiecesGenerator
    {
        private string[,] _board;
        private int _x;
        private int _y;

        public PiecesGenerator(string[,] board, int x, int y)
        {
            this._board = board;
            this._x = x;
            this._y = y;
        }

        public PieceEnum GetPieces(PiecesType type)
        {
            return (new Pieces(this._board, this._x, this._y, type)).GetEnumerator();
        }

        public List<PieceEnum> GetAllPieces()
        {
            return new List<PieceEnum>
            {
                GetPieces(PiecesType.Type_1),
                GetPieces(PiecesType.Type_2),
                GetPieces(PiecesType.Type_3),
                GetPieces(PiecesType.Type_4),
                GetPieces(PiecesType.Type_5),
                GetPieces(PiecesType.Type_6),
                GetPieces(PiecesType.Type_7),
                GetPieces(PiecesType.Type_8)
            };
        }
    }

    public class Pieces : IEnumerable
    {
        private string[,] _borad;
        private int _x = -1;
        private int _y = -1;
        private PiecesType _type;

        public Pieces(string[,] borad, int x, int y, PiecesType type)
        {
            this._borad = borad;
            this._x = x;
            this._y = y;
            this._type = type;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public PieceEnum GetEnumerator()
        {
            return new PieceEnum(this._borad, this._x, this._y, this._type);
        }
    }

    public class PieceEnum : IEnumerator
    {
        private string[,] _borad;
        private int InitX = -1;
        private int InitY = -1;
        private int PositionX = -1;
        private int PositionY = -1;
        private PiecesType _Type;
        public PieceEnum(string[,] borad, int x, int y, PiecesType _type)
        {
            this._borad = borad;
            this.InitX = x;
            this.InitY = y;
            this.PositionX = x;
            this.PositionY = y;
            this._Type = _type;
        }

        public bool MoveNext()
        {
            switch (this._Type)
            {
                case PiecesType.Type_1:
                {
                    this.PositionY--;
                    break;
                }
                case PiecesType.Type_2:
                {
                    this.PositionX++;
                    this.PositionY--;
                    break;
                }
                case PiecesType.Type_3:
                {
                    this.PositionX++;
                    break;
                }
                case PiecesType.Type_4:
                {
                    this.PositionX++;
                    this.PositionY++;
                    break;
                }
                case PiecesType.Type_5:
                {
                    this.PositionY++;
                    break;
                }
                case PiecesType.Type_6:
                {
                    this.PositionX--;
                    this.PositionY++;
                    break;
                }
                case PiecesType.Type_7:
                {
                    this.PositionX--;
                    break;
                }
                case PiecesType.Type_8:
                {
                    this.PositionX--;
                    this.PositionY--;
                    break;
                }
            }
            return IsLegalIndex;
        }

        public void Reset()
        {
            this.PositionX = this.InitX;
            this.PositionY = this.InitY;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public string Current
        {
            get
            {
                return this._borad[this.PositionY, this.PositionX];
            }
        }

        public (int x, int y) CurrentPosition
        {
            get
            {
                return (this.PositionX, this.PositionY);
            }
        }

        public bool IsLegalIndex
        {
            get
            {
                return -1 < this.PositionX
                && -1 < this.PositionY
                && 8 > this.PositionX
                && 8 > this.PositionY;
            }
        }
    }
}