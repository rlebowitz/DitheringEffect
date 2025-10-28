/* Even more algorithms for dithering images using C#
 * https://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

/*
 * Stucki Dithering
 * http://www.efg2.com/Lab/Library/ImageProcessing/DHALF.TXT
 *
 *                  *  8/42 4/42
 *      2/42 4/42 8/42 4/42 2/42
 *      1/42 2/42 4/42 2/42 1/42
 */

using System.ComponentModel;

namespace Dithering.Algorithms
{
    [Description("Stucki")]
    public sealed class StuckiDithering : ErrorDiffusionDithering
    {
        #region Constructors

        public StuckiDithering()
          : base(new float[,]
                 {
               { 0, 0, 0, 8.0f/42.0f, 4.0f/42.0f },
               { 2.0f/42.0f, 4.0f/42.0f, 8.0f/42.0f, 4.0f/42.0f, 2.0f/42.0f },
               { 1.0f/42.0f, 2.0f/42.0f, 4.0f/42.0f, 2.0f/42.0f, 1.0f/42.0f }
                 })
        { }

        #endregion
    }
}