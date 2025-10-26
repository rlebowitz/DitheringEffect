#nullable disable

using Dithering.Algorithms;
using System.Linq.Expressions;

namespace Dithering
{
    public static class DitheringCollection
    {
        public static readonly ErrorDiffusionDithering[] Ditherings =
        {
                // Floyd–Steinberg
                new FloydSteinbergDithering(),
                // Jarvis, Judice and Ninke
                new JarvisJudiceNinkeDithering(),
                // Stucki
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
