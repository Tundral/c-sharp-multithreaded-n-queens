using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Queens.ConsoleHelpers;
using static System.Console;

namespace Queens
{
    class BoardDrawer
    {
        public static char QueenChar { get; set; } = 'X';

        public Board Board { get; }
        public (int Left, int Top) CornerPos { get; }
        public (int Left, int Top) BoardCornerPos { get; }
        public BoardDrawer(Board board, (int Left, int Top) cornerPos)
        {
            Board = board;
            CornerPos = cornerPos;
            BoardCornerPos = (cornerPos.Left + 1, cornerPos.Top + Board.Size);
        }
        
        public void DrawQueen(Queen q)
        {
            DrawString((BoardCornerPos.Left + q.X - 1, BoardCornerPos.Top - q.Y + 1), QueenChar.ToString());
        }

        public void RedrawMovedQueen((int X, int Y) oldpos, Queen q)
        {
            DrawString((BoardCornerPos.Left + oldpos.X - 1, BoardCornerPos.Top - oldpos.Y + 1), " ");
            DrawQueen(q);
        }

        public void OnQueeenMoved(object sender, Queen.QueenMovedEventArgs args)
        {
            RedrawMovedQueen((args.PreviousX, args.PreviousY), (Queen)sender);
        }
        public void OnQueenRemoved(object sender, Board.QueenRemovedEventArgs args)
        {
            Queen q = args.Queen;
            DrawString((BoardCornerPos.Left + q.X - 1, BoardCornerPos.Top - q.Y + 1), " ");
        }

        public void Draw()
        {
            LinkedList<Queen> queens = Board.Queens;
            int size = Board.Size;

            

            string index = BoardDrawerManager.DrawerManager.Drawers.IndexOf(this).ToString();
            DrawString(CornerPos, $"{index}{new String('═', size - index.Length + 1)}╗");

            for (int i = 1; i < size + 1; i++)
            {
                DrawString((CornerPos.Left, CornerPos.Top + i), $"║{new String(' ', size)}║");
            }
            DrawString((CornerPos.Left, CornerPos.Top + size + 1), $"╚{new String('═', size)}╝");


            if (queens.Count < 1) return;

            Queen firstQueen = queens.First.Value;

            DrawQueen(queens.First());

            if (queens.Count < 2) return;

            var curNode = queens.First.Next;

            do
            {
                DrawQueen(curNode.Value);
                curNode = curNode.Next;
            } while (curNode is not null);


            
        }

    }
}
