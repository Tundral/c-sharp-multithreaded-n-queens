using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using static System.Math;
using System.Threading;
using MoreLinq;
using MoreLinq.Extensions;

namespace Queens
{
    class Board : ICloneable, IEquatable<Board>
    {
        //Field variables/properties
        public int Size { get;}
        public LinkedList<Queen> Queens{ get; }


        //Constructors
        public Board(int size)
        {
            Size = size;
            Queens = new LinkedList<Queen>();
        }

        public Board(int[] xPos)
        {
            Size = xPos.Length;
            Queens = new LinkedList<Queen>();

            int y = 1;
            foreach (int item in xPos)
            {
                AddQueen(item, y);
                y++;
            }
        }


        //General Methods

        public object Clone()
        {
            var clonedBoard = new Board(this.Size);

            foreach (var queen in Queens) {
                clonedBoard.AddQueen(queen.X, queen.Y); 
            }

            return clonedBoard;
        }
        public void Print()
        {
            Console.WriteLine($"Board ${base.GetHashCode()}");
            foreach (var queen in this.Queens) Console.WriteLine(queen + " " + queen.GetHashCode());

            Console.WriteLine($"╔{new String('═', Size)}╗");
            for (int i = 0; i < Size; i++)
            {
                Console.WriteLine($"║{new String(' ', Size)}║");
            }
            Console.Write($"╚{new String('═', Size)}╝");

            ConsoleHelpers.MoveCursor(-1 * (Size + 1), 1);

            var cornerPos = Console.GetCursorPosition();


            Queen firstQueen = Queens.First.Value;


            ConsoleHelpers.MoveCursor(firstQueen.X - 1, firstQueen.Y - 1);
            Console.Write("X");

            if (Queens.Count < 2) return;

            var curNode = Queens.First.Next;
            Queen curQueen;

            do
            {
                curQueen = curNode.Value;

                Console.SetCursorPosition(cornerPos.Left, cornerPos.Top);

                ConsoleHelpers.MoveCursor(curQueen.X - 1 , curQueen.Y -1);
                Console.Write("X");
                
                curNode = curNode.Next;
            } while (curNode is not null);

            Console.SetCursorPosition(cornerPos.Left - 1, cornerPos.Top + 3);


        }
        public bool Equals(Board that)
        {
            if (that is null) return false;
            if (GetHashCode() == that.GetHashCode()) return true;
            if (Queens.Count != that.Queens.Count) return false;

            var thisCoords = from q in Queens
                             orderby q.X, q.Y
                             select new { X = q.X, Y = q.Y };

            var thatCoords = from q in that.Queens
                             orderby q.X, q.Y
                             select new { X = q.X, Y = q.Y };

            for (int i = 0; i < thisCoords.Count(); i++)
            {
                if (!thisCoords.ElementAt(i).Equals(thatCoords.ElementAt(i))) return false;
            }
                return true;
            }


        //Mirror and rotation methods 

        public void HorizontalMirror()
        {
            foreach(Queen queen in Queens)
            {
                queen.X = Size - queen.X + 1;
            }
        }

        public void VerticalMirror()
        {
            foreach (Queen queen in Queens)
            {
                queen.Y = Size - (Abs(queen.Y-1));
            }
        }

        public void Rotate90()
        {
            foreach (Queen queen in Queens)
            {
                int origY = queen.Y;
                queen.Y = Size - queen.X + 1;
                queen.X = origY;
            }
        }

        public bool AnyRotationMirror(Board that)
        {
            if (GetHashCode() == that.GetHashCode()) return false;
            
            Board thatClone = (Board)that.Clone();

            if (this.Equals(thatClone)) return true;
            thatClone.Rotate90();
            if (this.Equals(thatClone)) return true;
            thatClone.Rotate90();
            if (this.Equals(thatClone)) return true;
            thatClone.Rotate90();
            if (this.Equals(thatClone)) return true;
            thatClone.Rotate90();

            thatClone.HorizontalMirror();
            if (this.Equals(thatClone)) return true;
            thatClone.Rotate90();
            if (this.Equals(thatClone)) return true;
            thatClone.Rotate90();
            if (this.Equals(thatClone)) return true;
            thatClone.Rotate90();
            if (this.Equals(thatClone)) return true;

            return false;

        }

        public int[] TrueRotation()
        {

            Board clone = (Board)Clone();

            List<Board> rotations = new List<Board>();

            rotations.Add((Board)clone.Clone());
            clone.Rotate90();
            rotations.Add((Board)clone.Clone());
            clone.Rotate90();
            rotations.Add((Board)clone.Clone());
            clone.Rotate90();
            rotations.Add((Board)clone.Clone());
            clone.Rotate90();

            clone.HorizontalMirror();
            rotations.Add((Board)clone.Clone());
            clone.Rotate90();
            rotations.Add((Board)clone.Clone());
            clone.Rotate90();
            rotations.Add((Board)clone.Clone());
            clone.Rotate90();
            rotations.Add((Board)clone.Clone());

                


            var xCoords = rotations.Select(r => r.Queens.OrderBy(q => q.Y).Select(q => q.X)).ToList();
            xCoords.Sort((a, b) =>
            {
                for (int i = 0; i < this.Queens.Count - 1; i++)
                {
                    int diff = a.ElementAt(i) - b.ElementAt(i);
                    if (diff == 0) continue;
                    else return diff;
                }
                return 0;
            });

            return xCoords.First().ToArray();
            
        }

        //Queen and validation methods

        public void AddQueen(int x, int y)
        {

            Queen nq = new Queen(x, y, this);
            Queens.AddLast(nq);
            
        }

        public bool IsValid()
        {
            foreach (var q in this.Queens)
            {
                foreach (var q2 in this.Queens)
                {
                    if (q.Threatens(q2)) return false;
                }
            }
            return true;
        }

        public bool TopQueenThreatens()
        {
            Queen topQ = Queens.Last();
            foreach (var q in this.Queens) if (topQ.Threatens(q)) return true;
            return false;
        }

        public void AddQueenToNextRow(int x)
        {
            AddQueen(x, Queens.Last.Value.Y + 1);
        }

        public bool TryRemoveTopQueen()
        {
            if (Queens.First.Next is null) return false;
            Queens.RemoveLast();
            return true;
        }

        public void MoveTopQueenToX(int x)
        {
            Queens.Last().X = x;
        }

        public bool QueensOnSeqRows()
        {
            if (Queens.Count < 1) return true;
            var rows = Enumerable.Range(1, Queens.Count).ToList();
            var remainder = Queens.Select(q => q.Y)
                                        .ToList()
                                        .Except(rows);
            return remainder.Count() < 1;
        }

        //Static elements

        public static List<Board> RemoveDuplicateBoards(List<Board> boards)
        {
            return boards.AsParallel()
                .Select(b => new { Board = b, TrueRotation = string.Join("", b.TrueRotation()) })
                .GroupBy(b => b.TrueRotation)
                .Select(g => g.First().Board)
                .ToList();
        }

        public static List<Board> PrintBoards
    }
}