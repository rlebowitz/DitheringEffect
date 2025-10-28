/* Even more algorithms for dithering images using C#
 * https://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

/*
 * Sierra Dithering
 * http://www.efg2.com/Lab/Library/ImageProcessing/DHALF.TXT
 *
 *                  *  5/32 3/32
 *      2/32 4/32 5/32 4/32 2/32
 *           2/32 3/32 2/32
 */

using System.ComponentModel;

namespace Dithering.Algorithms
{
    [Description("Sierra3")]
    public sealed class Sierra3Dithering : ErrorDiffusionDithering
    {
        #region Constructors

        public Sierra3Dithering()
          : base(new float[,]
                 {
               { 0, 0, 0, 5.0f/32.0f, 3.0f/32.0f },
               { 2.0f/32.0f, 4.0f/32.0f, 5.0f/32.0f, 4.0f/32.0f, 2.0f/32.0f },
               { 0, 2.0f/32.0f, 3.0f/32.0f, 2.0f/32.0f, 0 }
                 })
        { }

        #endregion
    }
}