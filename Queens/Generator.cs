using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens
{
    class Generator
    {
        public Board Board { get;}
        public int UpperLimit { get; set; }
        public bool AlreadyReturned { get; set; } = false;

        public Generator(int size, int upperLimit)
        {
            Board = new Board(size);
            Board.AddQueen(1, 1);
            Board.AddQueen(3, 2);
            if (upperLimit < 1) throw new ArgumentException("Upper limit must be 2 or higher");
            UpperLimit = upperLimit;
        }

        public Board Next()
        {
            LinkedList<Queen> theQueens = Board.Queens;
            while (true)
            {

                if (Board.TopQueenThreatens() || AlreadyReturned)
                {
                    while (!theQueens.Last().TryMove(1, 0))
                    {
                        if (!Board.TryRemoveTopQueen()) return null;
                    }
                    
                    AlreadyReturned = false;
                
                }
                else
                {
                    if (theQueens.Count == UpperLimit) break;
                    Board.AddQueen(1, theQueens.Last().Y + 1);
                }    
            }

            AlreadyReturned = true;

            return (Board)Board.Clone();

        }

        public List<Board> Generate()
        {
            List<Board> retVal = new List<Board>();

            Board solution = Next();
            while (solution is not null)
            {
                retVal.Add(solution);
                solution = Next();
            }

            return retVal;
        }
    }
}
