using PaintDotNet.Imaging;
using System.Collections.Generic;

namespace Dithering.Palettes
{
    public class Palette(ColorBgra32[] colors) : IPalette
    {
        private ColorBgra32[] Colors { get; set; } = colors;
        private Dictionary<ColorBgra32, ColorBgra32> Cache { get; set; }
        public void Clear()
        {
            Cache.Clear();
        }
        public ColorBgra32 FindClosestColor(ColorBgra32 color)
        {
            if (Cache.TryGetValue(color, out var cachedColor))
            {
                return cachedColor;
            }
            int index = 0;
            var minDistance = int.MaxValue;
            for (int i = 0; i < Colors.Length; i++)
            {
                var distance = SquaredDistanceTo(color, Colors[i]);
                if (distance < minDistance)
                {
                    index = i;
                    minDistance = distance;
                }
            }
            Cache.Add(color, Colors[index]);
            return Colors[index];
        }

        public static int SquaredDistanceTo(ColorBgra32 colorA, ColorBgra32 colorB)
        {
            var Gdiff = colorA.G - colorB.G;
            var Bdiff = colorA.B - colorB.B;
            var Rdiff = colorA.R - colorB.R;
            return Gdiff * Gdiff + Bdiff * Bdiff + Rdiff * Rdiff;
        }

    }
}

