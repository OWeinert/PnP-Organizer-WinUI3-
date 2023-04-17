using Microsoft.UI;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Windows.UI;

namespace PnPOrganizer.Core
{
    public static class Palette
    {
        private static ImmutableList<Color>? _paletteColors;
        public static ImmutableList<Color> PaletteColors
        {
            get
            {
                if (_paletteColors == null)
                {
                    var colors = new List<Color>();
                    foreach (var field in typeof(Palette).GetFields(BindingFlags.Static | BindingFlags.Public))
                    {
                        var color = (Color)field.GetValue(null)!;
                        colors.Add(color);
                    }
                    _paletteColors =  colors.ToImmutableList();
                }
                return _paletteColors;
            }
        }

        public static readonly Color Violet = Colors.DarkOrchid;
        public static readonly Color Magenta = Colors.HotPink;
        public static readonly Color Pink = Colors.LightPink;

        public static readonly Color Red = Colors.Firebrick;
        public static readonly Color LightRed = Colors.Coral;
        public static readonly Color Orange = Colors.OrangeRed;
        public static readonly Color Yellow = Colors.Gold;
        public static readonly Color Brown = Colors.SaddleBrown;

        public static readonly Color Green = Colors.MediumSeaGreen;
        public static readonly Color LightGreen = Colors.LightGreen;

        public static readonly Color Blue = Colors.SteelBlue;
        public static readonly Color LightBlue = Colors.LightSkyBlue;
        public static readonly Color BlueGray = Colors.LightSteelBlue;
        
        public static readonly Color White = Colors.White;
        public static readonly Color Black = Colors.Black;
        public static readonly Color DarkGray = Colors.DimGray;
        public static readonly Color LightGray = Colors.LightGray;
    }
}
