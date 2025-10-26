#nullable disable

using Dithering.Palettes;

namespace Dithering
{

    public static class PaletteCollection
    {
        public static readonly Palette[] Palettes =
        [
                new BlackAndWhitePalette(),
                new Windows16Palette(),
                new Windows20Palette(),
                new Apple16Palette(),
                new RiscOSPalette()
            ];
    }
}
