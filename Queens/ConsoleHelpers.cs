using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace Queens
{
    class ConsoleHelpers
    {
        private static readonly object balanceLock = new object();

        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        public static void Maximize()
        {
            ConsoleHelpers.ShowWindow(ThisConsole, MAXIMIZE);
        }

        public static void MoveCursor(int x, int y)
        {

            var currPos = Console.GetCursorPosition();
            Console.SetCursorPosition(currPos.Left + x, currPos.Top - y);
        }
        public static void DrawString((int Left, int Top) pos, string output)
        {
            lock (balanceLock)
            {
                Console.SetCursorPosition(pos.Left, pos.Top);
                Console.Write(output);
            }
        }

        private static void SetCursor((int Left, int Top) pos)
        {
            lock (balanceLock)
            {
                Console.SetCursorPosition(pos.Left, pos.Top);

            }
            
        }
    }
}
