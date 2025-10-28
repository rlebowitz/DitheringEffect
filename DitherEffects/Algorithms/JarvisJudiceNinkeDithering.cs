/* Even more algorithms for dithering images using C#
 * https://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

/*
 * Jarvis, Judice & Ninke Dithering
 * http://www.efg2.com/Lab/Library/ImageProcessing/DHALF.TXT
 *
 *                  *  8/48 5/48
 *      3/48 5/48 7/48 5/48 3/48
 *      1/48 3/48 5/48 3/48 1/48
 */

using System.ComponentModel;

namespace Dithering.Algorithms
{
    [Description("Jarvis, Judice & Ninke")]
    public sealed class JarvisJudiceNinkeDithering : ErrorDiffusionDithering
    {
        #region Constructors

        public JarvisJudiceNinkeDithering()
          : base(new float[,]
                 {
               { 0, 0, 0, 7.0f/48.0f, 5.0f/48.0f },
               { 3.0f/48.0f, 5.0f/48.0f, 7.0f/48.0f, 5.0f/48.0f, 3.0f/48.0f },
               { 1.0f/48.0f, 3.0f/48.0f, 5.0f/48.0f, 3.0f/48.0f, 1.0f/48.0f }
                 })
        { }

        #endregion
    }
}