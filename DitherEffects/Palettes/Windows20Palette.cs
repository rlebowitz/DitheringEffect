using System.Drawing;
using PaintDotNet.Imaging;

namespace Dithering.Palettes
{
    public class Windows20Palette() : Palette([
                    Color.Black,
                    Color.Maroon,
                    Color.Green,
                    Color.Olive,
                    Color.Navy,
                    Color.Purple,
                    Color.Teal,
                    Color.Silver,
                    Color.FromArgb(255,192, 192, 192),
                    Color.FromArgb(255, 240, 240, 240),
                    Color.FromArgb(255, 240, 240, 240),
                    Color.FromArgb(255, 164, 164, 164),
                    Color.Gray,
                    Color.Red,
                    Color.Lime,
                    Color.Yellow,
                    Color.Blue,
                    Color.Fuchsia,
                    Color.Aqua,
                    Color.White
                ])
    {
    }
}
