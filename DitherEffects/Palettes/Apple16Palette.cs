#nullable disable

using PaintDotNet;
using PaintDotNet.Imaging;

namespace Dithering.Palettes
{
    public class Apple16Palette():Palette([
                    SrgbColors.White,
                    ColorBgra32.FromBgra(5, 243, 251,255),
                    ColorBgra32.FromBgra(3, 100, 255, 255),
                    ColorBgra32.FromBgra(7, 9, 221, 255),
                    ColorBgra32.FromBgra(132, 8, 242, 255),
                    ColorBgra32.FromBgra(165, 0, 71, 255),
                    ColorBgra32.FromBgra(211, 0, 0, 255),
                    ColorBgra32.FromBgra(234, 171, 2, 255),
                    ColorBgra32.FromBgra(20, 183, 31, 255),
                    ColorBgra32.FromBgra(18, 100, 0, 255),
                    ColorBgra32.FromBgra(5, 44, 86, 255),
                    ColorBgra32.FromBgra(58, 113, 144, 255),
                    SrgbColors.Silver,
                    SrgbColors.Gray,
                    ColorBgra32.FromBgra(64, 64, 64, 255),
                    SrgbColors.Black
                ])
    {
    }
}
