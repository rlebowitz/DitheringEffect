/* Dithering an image using the Floyd–Steinberg algorithm in C#
 * https://www.cyotek.com/blog/dithering-an-image-using-the-floyd-steinberg-algorithm-in-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using PaintDotNet;
using System.Drawing;

namespace Dithering.Algorithms
{
    public interface IErrorDiffusion
    {
        #region Methods

        void Diffuse(Surface data, ColorBgra original, int x, int y, Rectangle rect);

        bool PreScan { get; }

        #endregion
    }
}