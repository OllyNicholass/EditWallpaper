using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Diagnostics;

namespace EditWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            Display.DisplayInfo();
            Console.WriteLine();

            var wallpaperPath = GetWallpaperPath();
            while (string.IsNullOrWhiteSpace(wallpaperPath))
            {
                wallpaperPath = GetWallpaperPath();
            }

            int? wallpaperStyle = null;
            wallpaperStyle = GetWallpaperStyle();
            while (wallpaperStyle == null)
            {
                wallpaperStyle = GetWallpaperStyle();
            }

            try
            {
                ChangeWallpaper(wallpaperPath, (int)wallpaperStyle);
            }
            catch (System.Security.SecurityException ex)
            {
                ConsoleExtensions.WriteLine(ex.Message, ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                ConsoleExtensions.WriteLine(ex.Message, ConsoleColor.Red);
            }

            var restartExplorer = RestartExplorer();
            while (restartExplorer == null)
            {
                restartExplorer = RestartExplorer();
            }

            ConsoleExtensions.WriteLine("It is now safe to close the application.", ConsoleColor.Yellow);
            Console.ReadLine();

        }

        private static int? RestartExplorer()
        {
            ConsoleExtensions.WriteLine("Windows Explorer needs to restart in order for this to take effect.", ConsoleColor.Yellow);
            ConsoleExtensions.Write("Do you wish to restart Explorer? [y, n]: ", ConsoleColor.Yellow);

            var input = Console.ReadLine();

            if (input.ToLower() == "y")
            {
                var processes = Process.GetProcesses();
                var explorer = processes.First(p => p.ProcessName == "explorer");
                explorer.Kill();
                Process.Start("explorer.exe");

                ConsoleExtensions.WriteLine("Windows Explorer has now been restarted.", ConsoleColor.Green);
                return 1;
            }
            else if (input.ToLower() == "n")
            {
                return 0;
            }
            else
            {
                ConsoleExtensions.WriteLine($"{input}, is not a valid input. Please enter, 'y' or 'n'", ConsoleColor.Red);
                return null;
            }
        }

        private static int? GetWallpaperStyle()
        {
            ConsoleExtensions.Write("Wallpaper Style: ", ConsoleColor.Yellow);
            dynamic input = Console.ReadLine();

            dynamic selected;

            // Check if input is an int or a string
            int value;
            if (int.TryParse(input, out value))
            {
                // Converts to an int
                input = value;
                selected = WallpaperStyle.Styles.FirstOrDefault(s => s.Key == input);
            }
            else
            {
                selected = WallpaperStyle.Styles.FirstOrDefault(s => s.Value.ToLower() == input.ToLower());
            }

            if (selected.Value == null)
            {
                ConsoleExtensions.WriteLine($"You have not selected a valid style. Please choose from the list below.", ConsoleColor.Red);
                Display.DisplayStyleGuideTable();
                return null;
            }

            ConsoleExtensions.WriteLine($"Style successfully selected: {selected.Value}", ConsoleColor.Green);
            return selected.Key;
        }

        private static string GetWallpaperPath()
        {
            ConsoleExtensions.Write("Wallpaper Location: ", ConsoleColor.Yellow);
            var input = Console.ReadLine();

            if (File.Exists(input))
            {
                var ext = System.IO.Path.GetExtension(input).ToLower();
                var supportedFileExtensions = new List<string>()
                {
                     ".bmp",
                     ".gif",
                     ".jpg",
                     ".png",
                     ".tif",
                     ".dib",
                     ".jfif",
                     ".jpe",
                     ".jpeg",
                     ".wdp"
                };

                if (supportedFileExtensions.Where(e => e.ToLower() == ext).Count() > 0)
                {
                    ConsoleExtensions.WriteLine($"File successfully selected: {input}", ConsoleColor.Green);
                    return input;
                }
                else
                {
                    ConsoleExtensions.WriteLine($"The {ext} extension is not supported. Please choose from the file types listed below and try again. ", ConsoleColor.Red);
                    foreach (var extension in supportedFileExtensions)
                    {
                        ConsoleExtensions.WriteLine(extension, ConsoleColor.DarkGray);
                    }
                    return "";
                }
            }
            else
            {
                ConsoleExtensions.WriteLine("This file does not exist or could not be found. Please check the path and try again.", ConsoleColor.Red);
                return "";
            }
        }

        static void ChangeWallpaper(string wallpaperPath, int wallpaperStyle)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
                key.SetValue("Wallpaper", wallpaperPath);
                key.SetValue("WallpaperStyle", wallpaperStyle);
                Console.WriteLine($"Wallpaper successfully changed to {wallpaperPath} : {wallpaperStyle}");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
