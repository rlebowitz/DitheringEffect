using PaintDotNet;
using System.Collections.Generic;

namespace Dithering.Palettes
{
    public class Palette(ColorBgra[] colors) : IPalette
    {
        private ColorBgra[] Colors { get; set; } = colors;
        private Dictionary<ColorBgra, ColorBgra> Cache { get; set; }
        public void Clear()
        {
            Cache.Clear();
        }
        public ColorBgra FindClosestColor(ColorBgra color)
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

        public static int SquaredDistanceTo(ColorBgra colorA, ColorBgra colorB)
        {
            var Gdiff = colorA.G - colorB.G;
            var Bdiff = colorA.B - colorB.B;
            var Rdiff = colorA.R - colorB.R;
            return Gdiff * Gdiff + Bdiff * Bdiff + Rdiff * Rdiff;
        }

    }
}

