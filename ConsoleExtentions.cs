using System;
using System.Collections.Generic;
using System.Text;

namespace EditWallpaper
{
    public static class ConsoleExtentions
    {
        public static void WriteLine(string str, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public static void Write(string str, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(str);
            Console.ResetColor();
        }
    }
}
