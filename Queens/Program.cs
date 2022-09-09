using MoreLinq;
using MoreLinq.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Queens.BoardDrawerManager;

namespace Queens
{
    class Program
    {
        public static int BoardSize { get; set; } = 12 ;
        public static int HalfWaySize { get =>  BoardSize % 2 == 0 ? BoardSize / 2 : (BoardSize / 2) + 1; }
        static void Main(string[] args) {
            Algo();
        }

        static void Testing()
        {
            Board board = new Board(new int[] { 3, 5, 8, 4, 6, 2, 7, 1 });

            Board board2 = new Board(new int[] { 1, 7, 2, 6, 4, 8, 5, 3 });

            Console.WriteLine("True rotation:");
            Console.WriteLine(String.Join(", ", board.TrueRotation()));

            board.HorizontalMirror();
            board.Rotate90();
            board.HorizontalMirror();
            board.Rotate90();

            Console.WriteLine("True rotation:");
            Console.WriteLine(String.Join(", ", board.TrueRotation()));

            Console.WriteLine("Board 2 True rotation:");
            Console.WriteLine(String.Join(", ", board2.TrueRotation()));

            Console.ReadLine();

            DrawerManager.SubmitBoards(Board.RemoveDuplicateBoards(new List<Board> { board, board2 }));
            DrawerManager.DrawBoards();
            DrawerManager.Print("");





        }

        static void Countdown()
        {
            Console.WriteLine();
            Console.Write("5");
            Thread.Sleep(800);
            for (int i = 4; i > 1; i--)
            {
                Console.Write($"{i}...");
                Thread.Sleep(800);
            }
            Console.Write("1");
            Thread.Sleep(800);
        }

        static void Algo()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ConsoleHelpers.Maximize();
            Console.CursorVisible = false;

            int halfWaySize = BoardSize % 2 == 0 ? BoardSize / 2 : (BoardSize / 2) + 1;

            var generator = new Generator(BoardSize, 3);

            //DrawerManager.SubmitBoards(new List<Board>() { generator.Board });
            //DrawerManager.DrawBoards();

            List<Board> startingBoards = new List<Board>();

            Board solution = generator.Next();
            while (solution is not null)
            {
                startingBoards.Add(solution);
                solution = generator.Next();
            }

            //Countdown();

            //DrawerManager.Print($"Removing duplicates from {startingBoards.Count} startingboards!");

            List<Board> noDuplicateStartingBoards = Board.RemoveDuplicateBoards(startingBoards);


            List<Board> boards = new List<Board>();


            //DrawerManager.SubmitBoards(noDuplicateStartingBoards);
            //DrawerManager.DrawBoards();

            var results = startingBoards.AsParallel().Select(s => new Solver(s).FindAllUniqueSolutions()).SelectMany(x => x).ToList();



            //DrawerManager.Print("Resulting boards: " + results.Count);

            //Countdown();



            //Countdown();

            List<Board> noDuplicates = Board.RemoveDuplicateBoards(results);

            stopWatch.Stop();

            DrawerManager.SubmitBoards(noDuplicates);
            DrawerManager.DrawBoards();

            DrawerManager.Print($"{noDuplicates.Count} solutions");
            

            List<List<Queen>> theSolutions = new List<List<Queen>>();
            noDuplicates.ForEach(b => theSolutions.Add(b.Queens.ToList()));

            theSolutions.ForEach(s => s.Sort( (thisQ , thatQ)  => thisQ.Y - thatQ.Y));

            theSolutions.ForEach(s =>
            {
                string str = string.Join(',', s.Select(s => s.X));
                str = string.Format("[{0}]", str);
                DrawerManager.Print(str);
            });
            DrawerManager.Print($" Time elapsed: {stopWatch.Elapsed}");


        }
    }
}
