#nullable disable

using PaintDotNet.Imaging;

namespace Dithering.Palettes
{
    public class Windows20Palette() : Palette([
                    SrgbColors.Black,
                    SrgbColors.Maroon,
                    SrgbColors.Green,
                    SrgbColors.Olive,
                    SrgbColors.Navy,
                    SrgbColors.Purple,
                    SrgbColors.Teal,
                    SrgbColors.Silver,
                    ColorBgra32.FromBgra(192, 192, 192, 255),
                    ColorBgra32.FromBgra(240, 240, 240, 255),
                    ColorBgra32.FromBgra(240, 240, 240, 255),
                    ColorBgra32.FromBgra(164, 164, 164, 255),
                    SrgbColors.Gray,
                    SrgbColors.Red,
                    SrgbColors.Lime,
                    SrgbColors.Yellow,
                    SrgbColors.Blue,
                    SrgbColors.Fuchsia,
                    SrgbColors.Aqua,
                    SrgbColors.White
                ])
    {
    }
}
