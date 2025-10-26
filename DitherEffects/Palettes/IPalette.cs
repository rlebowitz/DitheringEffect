#nullable disable

using PaintDotNet.Imaging;

namespace Dithering.Palettes
{
    public interface IPalette
    {
        public ColorBgra32 FindClosestColor(ColorBgra32 color);
        public void Clear();
    }
}

