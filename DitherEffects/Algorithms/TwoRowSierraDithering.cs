﻿/* Even more algorithms for dithering images using C#
 * https://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

/*
 * Two Row Sierra Dithering
 * http://www.efg2.com/Lab/Library/ImageProcessing/DHALF.TXT
 *
 *                  *  4/16 3/16
 *      1/16 2/16 3/16 2/16 1/16
 */

using System.ComponentModel;

namespace Dithering.Algorithms
{
    [Description("Two-Row Sierra")]
    public sealed class TwoRowSierraDithering : ErrorDiffusionDithering
    {
        #region Constructors

        public TwoRowSierraDithering()
          : base(new byte[,]
                 {
               {
                 0, 0, 0, 4, 3
               },
               {
                 1, 2, 3, 2, 1
               }
                 }, 4, true)
        { }

        #endregion
    }
}