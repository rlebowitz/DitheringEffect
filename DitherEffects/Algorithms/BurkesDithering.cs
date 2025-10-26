﻿/* Dithering an image using the Burkes algorithm in C#
 * https://www.cyotek.com/blog/dithering-an-image-using-the-burkes-algorithm-in-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

/*
 * Burkes Dithering
 * http://www.efg2.com/Lab/Library/ImageProcessing/DHALF.TXT
 *
 *                  *  8/32 4/32
 *      2/32 4/32 8/32 4/32 2/32
 */

#nullable disable

using System.ComponentModel;

namespace Dithering.Algorithms
{
    [Description("Burkes")]
    public sealed class BurkesDithering : ErrorDiffusionDithering
    {
        #region Constructors

        public BurkesDithering()
          : base(new byte[,]
                 {
               {
                 0, 0, 0, 8, 4
               },
               {
                 2, 4, 8, 4, 2
               }
                 }, 5, true)
        { }

        #endregion
    }
}