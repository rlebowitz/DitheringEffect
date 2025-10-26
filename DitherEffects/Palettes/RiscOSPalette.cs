#nullable disable

using PaintDotNet;
using PaintDotNet.Imaging;

namespace Dithering.Palettes
{
    public class RiscOSPalette():Palette([
                    SrgbColors.White,
                    ColorBgra32.FromBgra(221, 221, 221, 255),
                    ColorBgra32.FromBgra(187, 187, 187, 255),
                    ColorBgra32.FromBgra(153, 153, 153, 255),
                    ColorBgra32.FromBgra(119, 119, 119, 255),
                    ColorBgra32.FromBgra(85, 85, 85, 255),
                    ColorBgra32.FromBgra(51, 51, 51, 255),
                    SrgbColors.Black,
                    ColorBgra32.FromBgra(153, 153, 153, 255),
                    ColorBgra32.FromBgra(0, 0, 0, 255),
                    ColorBgra32.FromBgra(0, 0, 0, 255),
                    ColorBgra32.FromBgra(0, 0, 0, 255),
                    ColorBgra32.FromBgra(187, 187, 187, 255),
                    ColorBgra32.FromBgra(0, 0, 0, 255),
                    ColorBgra32.FromBgra(0, 0, 0, 255),
                    ColorBgra32.FromBgra(255, 255, 255, 255)
                ])
    {
    }
}
