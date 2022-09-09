using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queens
{


    class BoardDrawerManager
    {
        public static BoardDrawerManager DrawerManager = new BoardDrawerManager();
        public List<BoardDrawer> Drawers { get; } = new List<BoardDrawer>();

        public int PrintsStartPos { get; set; }
        public int PrintRows { get; set; } = 0;
        public int BoardSize { get; set; }
        public int Spacing { get; set; }
        public int BoardsInRow { get; set; }
        public int SingleBoardSpace { get; set; }

        public void SubmitBoards(List<Board> boards)
        {
            Clear();

            System.Console.CursorVisible = false;

            BoardSize = boards[0].Size;
            Spacing = 4;
            SingleBoardSpace = BoardSize + Spacing;
            BoardsInRow = Console.WindowWidth / SingleBoardSpace;

            if (boards.Count < 1) return;

            for (int i = 0; i < boards.Count ; i++)
            {
                int posInSeq = i;
                int row = posInSeq / BoardsInRow + 1;
                int posInRow = posInSeq % BoardsInRow;

                Board board = boards[i];
                
                BoardDrawer drawer = new BoardDrawer(board, ((posInRow) * SingleBoardSpace, (row - 1) * SingleBoardSpace));
                board.Drawer = drawer;
                board.queenRemovedEvent += drawer.OnQueenRemoved;
                board.Queens.ToList().ForEach(q => q.queenMovedEvent += drawer.OnQueeenMoved);


                Drawers.Add(drawer);

            }
        }

        public void DrawBoards()
        {
            Console.Clear();
            Drawers.ForEach(d => d.Draw());
            PrintsStartPos = Drawers.Select(d => d.BoardCornerPos.Top).Max() +2;
        }

        public void Print(string output)
        {
            ConsoleHelpers.DrawString((0, PrintsStartPos + PrintRows), output);
            int numLines = output.Split('\n').Length;
            PrintRows += numLines;

        }
        
        public void Clear()
        {
            DisableDynamicDrawing();

            Drawers.Clear();

            Console.Clear();
        }
        public void ClearPrints()
        {
            Console.SetCursorPosition(0, PrintsStartPos);
            for (int i = 0; i < PrintRows; i++)
            {
                Console.WriteLine(new string(' ', Console.BufferWidth));
            }
            PrintRows = 0;
            Console.SetCursorPosition(0, PrintsStartPos);
        }

        public void DisableDynamicDrawing()
        {
            Drawers.ForEach(d =>
            {
                d.Board.queenRemovedEvent -= d.OnQueenRemoved;
                d.Board.Queens.ToList().ForEach(q =>
                {
                    q.queenMovedEvent -= d.OnQueeenMoved;
                });
            });
        }

        public void EnableDynamicDrawing()
        {
            Drawers.ForEach(d =>
            {
                d.Board.queenRemovedEvent += d.OnQueenRemoved;
                d.Board.Queens.ToList().ForEach(q =>
                {
                    q.queenMovedEvent += d.OnQueeenMoved;
                });
            });
        }











    }
}
