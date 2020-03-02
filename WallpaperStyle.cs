using System.Collections.Generic;

namespace EditWallpaper
{
    public static class WallpaperStyle
    {
        public static Dictionary<int, string> Styles
        {
            get
            {
                return new Dictionary<int, string>()
                {
                    { 0, "Center" },
                    { 1, "Tile" },
                    { 2, "Stretch" },
                    { 3, "Fit" },
                    { 4, "Fill" },
                    { 5, "Span" }
                };
            }
        }
    }
}