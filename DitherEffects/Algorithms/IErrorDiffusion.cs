/* Dithering an image using the Floyd–Steinberg algorithm in C#
 * https://www.cyotek.com/blog/dithering-an-image-using-the-floyd-steinberg-algorithm-in-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

#nullable disable

using PaintDotNet;
using PaintDotNet.Imaging;
using PaintDotNet.Rendering;

namespace Dithering.Algorithms
{
    public interface IErrorDiffusion
    {
        #region Methods

        public void Diffuse(RegionPtr<ColorBgra32> data, ColorBgra32 original, ColorBgra32 transformed, int x, int y, RectInt32 bounds);

        public bool PreScan { get; }

        #endregion
    }
}