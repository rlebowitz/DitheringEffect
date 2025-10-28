#nullable disable

using Dithering;
using Dithering.Algorithms;
using System.Linq.Expressions;

namespace Dithering
{
    //https://github.com/cyotek/Dithering/tree/master/src
    public static class DitheringCollection
    {
        public static readonly ErrorDiffusionDithering[] Ditherings =
        {
                // Floyd–Steinberg
                new FloydSteinbergDithering(),
                // Jarvis, Judice and Ninke
                new JarvisJudiceNinkeDithering(),
                // Atkinson
                new AtkinsonDithering(),
                // 4-cell Shiau-Fan
                new FourCellShiauFanDithering(),
                // 5-cell Shiau-Fan
                new FiveCellShiauFanDithering(),
                // Stucki
                new StuckiDithering(),
                // Burkes
                new BurkesDithering(),
                // Sierra
                new Sierra2Dithering(),
                // Three-row Sierra
                new Sierra3Dithering(),
                // Sierra Lite
                new SierraLiteDithering(),
            };
    }
}
