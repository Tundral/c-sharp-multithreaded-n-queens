using MoreLinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queens
{
    class Solver
    {
        public Board Board { get; }
        public int MinHeight { get; }
        public int? UpperLimit { get; } = null;
        public LinkedList<Queen> Queens { get => Board.Queens; }
        public Queen TopQueen { get => Queens.Last(); }


        public Solver(Board board)
        {
            if (!board.QueensOnSeqRows()) throw new ArgumentException("Solver board queens must be on sequential rows");
            Board = board;
            if (Queens.Count < 1) Board.AddQueen(1, 1);
            else MinHeight = Queens.Select(q => q.Y).Max();

        }
        public Solver(Board board, int upperLimit)
            : this(board)
        {
            UpperLimit = upperLimit;
        }



        public Board FindSolution()
        {
            while (true)
            {
                if (Board.TopQueenThreatens())
                {
                    while(!TopQueen.TryMove(1,0))
                    {
                        if (!Board.TryRemoveTopQueen()) return null;
                    }
                }
                else
                {
                    if (Queens.Count == Board.Size && Board.IsValid()) break;
                    Board.AddQueen(1, TopQueen.Y + 1);
                }
            }
            return Board;
        }

        public List<Board> FindAllUniqueSolutions()
        {
            List<Board> retVal = new List<Board>();

            if (Queens.Count < 1) Board.AddQueen(1, 1);
            while (true)
            {
                if (Board.TopQueenThreatens())
                {
                    while (!TopQueen.TryMove(1, 0))
                    {
                        if (!TryRemoveTopQueen()) return retVal;
                    }
                }
                else
                {
                    if (Queens.Count == Board.Size && !Board.TopQueenThreatens()) 
                    {
                        Board board = (Board)Board.Clone();
                        AddIfUnique(retVal, board);
                        while (!TopQueen.TryMove(1, 0))
                        {
                            if (!TryRemoveTopQueen()) return retVal;
                        }
                    }
                    else Board.AddQueen(1, TopQueen.Y + 1);
                }
            }
            return retVal;
        }

        public List<Board> FindAllSolutions()
        {
            List<Board> retVal = new List<Board>();

            if (Queens.Count < 1) Board.AddQueen(1, 1);

            while (true)
            {
                //Thread.Sleep(50);
                if (Board.TopQueenThreatens())
                {
                    while (!TopQueen.TryMove(1, 0))
                    {
                        if (!TryRemoveTopQueen()) return retVal;
                    }
                }
                else
                {
                    if (Queens.Count == Board.Size && !Board.TopQueenThreatens())
                    {
                        Board board = (Board)Board.Clone();
                        retVal.Add(board);
                        while (!TopQueen.TryMove(1, 0))
                        {
                            if (!TryRemoveTopQueen()) return retVal;
                        }
                    }
                    else { 
                        Board.AddQueen(1, TopQueen.Y + 1); 
                    }
                }
            }
            return retVal;
        }

        public List<Board> FindAllPartialSolutions()
        {
            List<Board> retVal = new List<Board>();

            if (Queens.Count < 1) Board.AddQueen(1, 1);

            while (true)
            {
                if (TopQueen.X == Board.Size && TopQueen.Y == UpperLimit)
                {
                    if (Board.TopQueenThreatens()) return retVal;
                    else
                    {
                        retVal.Add((Board)Board.Clone());
                        return retVal;
                    }
                }
                Thread.Sleep(50);
                if (Board.TopQueenThreatens())
                {
                    while (!TopQueen.TryMove(1, 0))
                    {
                        if (!TryRemoveTopQueen()) return retVal;
                    }
                }
                else
                {
                    if (TopQueen.Y == UpperLimit && !Board.TopQueenThreatens())
                    {
                        BoardDrawerManager.DrawerManager.Print("Yaay, a solution!");
                        Board board = (Board)Board.Clone();
                        retVal.Add(board);
                        while (!TopQueen.TryMove(1, 0))
                        {
                            if (!TryRemoveTopQueen()) return retVal;
                        }
                    }
                    else
                    {
 
                        Board.AddQueen(1, TopQueen.Y + 1);
                    }
                }
            }
            return retVal;
        }



        public bool TryRemoveTopQueen()
        {
            Queen top = TopQueen;
            if (top.Y <= MinHeight + 1) return false;
            else return Board.TryRemoveTopQueen();
        }

        public static void AddIfUnique(List<Board> list, Board board)
        {
            foreach (Board b in list)
            {
                if (board.AnyRotationMirror(b)) return;
            }
            list.Add(board);
        }
    }
}
