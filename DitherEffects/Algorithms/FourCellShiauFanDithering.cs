/* Dithering an image using the Floyd–Steinberg algorithm in C#
 * https://www.cyotek.com/blog/dithering-an-image-using-the-floyd-steinberg-algorithm-in-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using System.ComponentModel;

namespace Dithering.Algorithms
{
    [Description("Four Cell Shiau Fan")]
    public sealed class FourCellShiauFanDithering : ErrorDiffusionDithering
    {
        #region Constructors

        public FourCellShiauFanDithering()
          : base(new float[,]
                {
                    { 0, 0, 0, 7.0f / 16.0f },
                    { 1.0f / 16.0f, 3.0f / 16.0f, 5.0f / 16.0f, 0 }
                })
        { }

        #endregion
    }
}