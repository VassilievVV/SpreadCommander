using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Extensions
{
    public static class ColorExtensions
    {
        public static Color Invert(this Color color)
        {
            var result = Color.FromArgb(color.ToArgb() ^ 0xFFFFFF);
            return result;
        }

        public static bool IsDark(this Color color)
        {
            return Convert.ToDouble(color.R + color.G + color.B) / (256.0 * 3.0) < 0.5;
        }

        public static string ToHexString(this Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

        public static string ToRgbString(this Color c) => $"RGB({c.R}, {c.G}, {c.B})";

        public static string ToHtmlColor(this Color color) => ColorTranslator.ToHtml(color);

        public static Color FromHtmlColor(string color) => FromHtmlColor(color, Color.Empty);

        public static Color FromHtmlColor(string color, Color defaultColor)
        {
            if (string.IsNullOrWhiteSpace(color) || int.TryParse(color, out int _) || double.TryParse(color, out double _))
                return defaultColor;

            Color result;
            try
            {
                result = ColorTranslator.FromHtml(color);
                if (result == Color.Empty)
                    result = defaultColor;
            }
            catch (Exception) 
            {
                result = defaultColor;
            }

            return result;
        }
    }
}
