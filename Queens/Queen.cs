using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
namespace Queens
{
    class Queen
    {

        //Field variables/properties
        public Board Board { get; }

        private int _x;
        private int _y;

        public int X
        {
            get => _x;
            set
            {
                int prevX = _x;
                if (value < 0 || value > Board.Size)
                    throw new ArgumentException($"Attempted to move queen from [{X},{Y}] to [{value},{Y}]: Out of bounds");
                _x = value;
                if (queenMovedEvent is not null) queenMovedEvent(this, new QueenMovedEventArgs(prevX, Y));
            }
        }

        public int Y
        {
            get => _y; 
            set
            {
                int prevY = _y;
                if (value < 0 || value > Board.Size) 
                    throw new ArgumentException($"Attempted to move queen from [{X},{Y}] to [{X},{value}]: Out of bounds");
                _y = value;

                if (queenMovedEvent is not null) queenMovedEvent(this, new QueenMovedEventArgs(X, prevY));
            } 
        }

        //Constructors
        public Queen(int x, int y, Board board)
        {
            Board = board;
            X = x;
            Y = y;
            
        }

        //Methods

        public bool TryMove(int x, int y)
        {
            int nY = Y + y, nX = X + x;

            if (nX > Board.Size) return false;
            if (nY > Board.Size) return false;

            X = nX;
            Y = nY;
            
            return true;
        }

        public bool Threatens(Queen that)
        {
            if (this == that) return false;
            if ((this.X == that.X) || (this.Y == that.Y)) return true;
            if (Abs(this.X - that.X) - Abs((this.Y - that.Y)) == 0) return true;
            return false;
        }
        public bool DiagonalThreatens(Queen that)
        {
            if (Abs(this.X - that.X) - Abs((this.Y - that.Y)) == 0) return true;
            return false;
        }


        public override string ToString()
        {
            return $"[ X:{X} Y:{Y} ]";
        }



        //Delegates and events

        public class QueenMovedEventArgs : EventArgs
        {
            public int PreviousX { get; }
            public int PreviousY { get; }

            public QueenMovedEventArgs(int previousX, int previousY)
            {
                PreviousX = previousX;
                PreviousY = previousY;
            }
        }

        public event EventHandler<QueenMovedEventArgs> queenMovedEvent;



    }
}
