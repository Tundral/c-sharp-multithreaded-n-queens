using MoreLinq;
using MoreLinq.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Queens
{
    class Program
    {
        public static int BoardSize { get; set; } = 9 ;
        static void Main(string[] args) {
            Algo();
        }

        static void Algo()
        {

            ConsoleHelpers.Maximize();
            Console.CursorVisible = false;


            Console.Write(@"======================================== ULTIMATE N-QUEENS SOLVER ========================================
Welcome to the ultimate n-queens solving program
The program will find all absolutely unique solutions to a given board size! (no rotations or mirrored versions allowed)

Be cautioned, board sizes over 12 will take some time to solve (>30s).

Please enter the board size you'd like to find solutions for: "

                );


            //==================== Asking user for board size ====================

            var currPos = Console.GetCursorPosition();
            int boardSize = ReadLineInt();
            while(boardSize < 4) { 
            
                Console.WriteLine("\nPlease enter a number that is four or greater!");
                
                Console.SetCursorPosition(currPos.Left, currPos.Top);
                Console.Write("                                ");
                Console.SetCursorPosition(currPos.Left, currPos.Top);
                
                boardSize = ReadLineInt();
            }

            List<Board> results;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Console.SetCursorPosition(currPos.Left, currPos.Top);
            Console.Write("                                ");

            //==================== Actual algorithm ====================
            //Parallel LinQ enables the use of multiple threads

            using (var spinner = new Spinner(currPos.Left, currPos.Top))
             {
             
                spinner.Start();


                results = new Generator(boardSize, 4).Generate();

                results = results.AsParallel().Select(s => new Solver(s).FindAllSolutions()).SelectMany(x => x).ToList();

                results = Board.RemoveDuplicateBoards(results);

                stopWatch.Stop();
             }

            //==================== Drawing the solutions ====================
            //Or count of solutions if there's too may of them to draw in the console

            List<List<Queen>> theSolutions = new List<List<Queen>>();
            results.ForEach(b => theSolutions.Add(b.Queens.ToList()));

            theSolutions.ForEach(s => s.Sort( (thisQ , thatQ)  => thisQ.Y - thatQ.Y));

            if(results.Count < 50)
            {
                foreach(Board res in results)
                {
                    res.Print();
                    Thread.Sleep(1000);
                }
            }

            Console.ReadLine();
            Console.ReadLine();


        }

        public static int ReadLineInt()
        {
            var currPos = Console.GetCursorPosition();
            string input;
            int retVal;
            do
            {
               Console.SetCursorPosition(currPos.Left, currPos.Top);
               Console.Write("                                ");
               Console.SetCursorPosition(currPos.Left, currPos.Top);
               input = Console.ReadLine();
            } while (!int.TryParse(input, out retVal));
            return retVal;
        }
    }
}
