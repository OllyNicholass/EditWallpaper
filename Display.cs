using System;

namespace EditWallpaper
{
    public class Display
    {
        static int tableWidth = 25;

        public static void DisplayInfo()
        {
            ConsoleExtentions.WriteLine("Wallpaper Changer v1.0.0", ConsoleColor.Yellow);
            Console.WriteLine();
            DisplayStyleGuideTable();

            // Correct Example
            Console.WriteLine();
            ConsoleExtentions.WriteLine("Example (Correct Usage)", ConsoleColor.Green);
            ConsoleExtentions.Write("\tWallpaper Location: ", ConsoleColor.Green);
            ConsoleExtentions.WriteLine("C:\\Wallpapers\\wallpaper.jpg", ConsoleColor.DarkGray);
            ConsoleExtentions.Write("\tWallpaper Style: ", ConsoleColor.Green);
            ConsoleExtentions.WriteLine("5", ConsoleColor.DarkGray);

            // Incorrect Example
            Console.WriteLine();
            ConsoleExtentions.WriteLine("Example (Incorrect Usage)", ConsoleColor.Red);
            ConsoleExtentions.Write("\tWallpaper Location: ", ConsoleColor.Red);
            ConsoleExtentions.WriteLine("https://i.ytimg.com/vi/hAq443fhyDo/maxresdefault.jpg", ConsoleColor.DarkGray);
            ConsoleExtentions.Write("\tWallpaper Style: ", ConsoleColor.Red);
            ConsoleExtentions.WriteLine("big", ConsoleColor.DarkGray);
        }

        public static void DisplayStyleGuideTable()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Style Guide");
            PrintLine();
            PrintRow("Style", "Value");
            PrintLine();
            foreach (var style in WallpaperSettings.Styles)
            {
                PrintRow(style.Value, style.Key.ToString());
            }
            PrintLine();
            Console.ResetColor();
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
