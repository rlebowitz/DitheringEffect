using PaintDotNet;

namespace Dithering.Palettes
{
    public interface IPalette
    {
        public ColorBgra FindClosestColor(ColorBgra color);
        public void Clear();
    }
}

