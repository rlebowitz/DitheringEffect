#nullable disable

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
                // Fan
                new FanDithering(),
                // 4-cell Shiau-Fan
                new FourCellShiauFanDithering(),
                // 5-cell Shiau-Fan
                new FiveCellShiauFanDithering(),
                new StuckiDithering(),
                // Burkes
                new BurkesDithering(),
                // Sierra
                new SierraDithering(),
                // Two-row Sierra
                new TwoRowSierraDithering(),
                // Sierra Lite
                new SierraLiteDithering(),
                // Atkinson
                new AtkinsonDithering()
            };
    }
}
