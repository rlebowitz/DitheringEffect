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
  *                  *  4/16 3/16
 *      1/16 2/16 3/16 2/16 1/16
 */

using System.ComponentModel;

namespace Dithering.Algorithms
{
    [Description("Sierra2")]
    public sealed class Sierra2Dithering : ErrorDiffusionDithering
    {
        #region Constructors

        public Sierra2Dithering()
          : base(new float[,]
             {
               { 0.0f/16.0f, 0.0f/16.0f, 0.0f/16.0f, 4.0f/16.0f, 3.0f/16.0f },
               { 1.0f/16.0f, 2.0f/16.0f, 3.0f/16.0f, 2.0f/16.0f, 1.0f/16.0f }
             })
        { }

        #endregion
    }
}