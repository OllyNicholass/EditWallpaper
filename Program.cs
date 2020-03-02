using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Linq;

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
            catch (System.Security.SecurityException)
            {
                ConsoleExtentions.WriteLine("You do not have access to the registry. Please try running this as an Administrator.", ConsoleColor.Red);
            }
        }

        private static int? GetWallpaperStyle()
        {
            ConsoleExtentions.Write("Wallpaper Style: ", ConsoleColor.Yellow);
            dynamic input = Console.ReadLine();

            dynamic selected;

            // Check if input is an int or a string
            int value;
            if (int.TryParse(input, out value))
            {
                // Converts to an int
                input = value;
                selected = WallpaperSettings.Styles.FirstOrDefault(s => s.Key == input);
            }
            else
            {
                selected = WallpaperSettings.Styles.FirstOrDefault(s => s.Value == input);
            }

            if (selected.Value == null)
            {
                ConsoleExtentions.WriteLine($"You have not selected a valid style. Please choose from the list below.", ConsoleColor.Red);
                Display.DisplayStyleGuideTable();
                return null;
            }

            ConsoleExtentions.WriteLine($"Style successfully selected: {selected.Value}", ConsoleColor.Green);
            return selected.Key;
        }

        private static string GetWallpaperPath()
        {
            ConsoleExtentions.Write("Wallpaper Location: ", ConsoleColor.Yellow);
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
                    ConsoleExtentions.WriteLine($"File successfully selected: {input}", ConsoleColor.Green);
                    return input;
                }
                else
                {
                    ConsoleExtentions.WriteLine($"The {ext} extension is not supported. Please choose from the file types listed below and try again. ", ConsoleColor.Red);
                    foreach (var extension in supportedFileExtensions)
                    {
                        ConsoleExtentions.WriteLine(extension, ConsoleColor.DarkGray);
                    }
                    return "";
                }
            }
            else
            {
                ConsoleExtentions.WriteLine("This file does not exist or could not be found. Please check the path and try again.", ConsoleColor.Red);
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
