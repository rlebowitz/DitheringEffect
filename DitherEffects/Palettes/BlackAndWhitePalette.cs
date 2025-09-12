using PaintDotNet.Imaging;

namespace Dithering.Palettes
{
    public class BlackAndWhitePalette() :
        Palette([
            ColorBgra32.FromBgra(0,0,0,255),
            ColorBgra32.FromBgra(255,255,255,255)])
    {
    }
}
