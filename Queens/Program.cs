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
        public static int BoardSize { get; set; } =  10 ;
        static void Main(string[] args) {
            Algo();
        }

        static void Algo()
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Board> results = new Generator(BoardSize, 4).Generate();

            results = results.AsParallel().Select(s => new Solver(s).FindAllUniqueSolutions()).SelectMany(x => x).ToList();

            results = Board.RemoveDuplicateBoards(results);

            stopWatch.Stop();

            

            DrawerManager.Print($"{results.Count} solutions");
            

            List<List<Queen>> theSolutions = new List<List<Queen>>();
            results.ForEach(b => theSolutions.Add(b.Queens.ToList()));

            theSolutions.ForEach(s => s.Sort( (thisQ , thatQ)  => thisQ.Y - thatQ.Y));

            if(theSolutions.Count < 5000)
            {
                theSolutions.ForEach(s =>
                {
                    string str = string.Join(',', s.Select(s => s.X));
                    str = string.Format("[{0}]", str);
                    DrawerManager.Print(str);
                });
            }

            DrawerManager.Print($"{results.Count} solutions");
            DrawerManager.Print($" Time elapsed: {stopWatch.Elapsed}");

            Console.ReadLine();
            Console.ReadLine();


        }
    }
}
