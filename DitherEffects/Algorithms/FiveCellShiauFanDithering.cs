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
    [Description("Five Cell Shiau Fan")]
    public sealed class FiveCellShiauFanDithering : ErrorDiffusionDithering
    {
        #region Constructors

        public FiveCellShiauFanDithering()
          : base(new float[,]
                {
                    { 1.0f / 16.0f, 4.0f / 16.0f, 2.0f / 16.0f },
                    { 1.0f / 16.0f, 0.0f / 16.0f, 1.0f / 16.0f }
                })
        { }

        #endregion
    }
}