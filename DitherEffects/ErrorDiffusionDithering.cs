/* Even more algorithms for dithering images using C#
 * https://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using PaintDotNet.Rendering;
using System;

namespace Dithering
{
    public interface IErrorDiffusion
    {
        #region Methods

        public void Diffuse(float[,] data, int x, int y, RectInt32 bounds, float threshold);

        public bool PreScan { get; }

        #endregion
    }

    public abstract class ErrorDiffusionDithering : IErrorDiffusion
    {
        #region Constants





        #endregion

        #region Constructors

        protected ErrorDiffusionDithering(float[,] matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            if (matrix.Length == 0)
            {
                throw new ArgumentException("Matrix is empty.", nameof(matrix));
            }

            Matrix = matrix;
            MatrixWidth = (byte)(matrix.GetUpperBound(1) + 1);
            MatrixHeight = (byte)(matrix.GetUpperBound(0) + 1);
  
            for (int i = 0; i < MatrixWidth; i++)
            {
                if (matrix[0, i] != 0)
                {
                    StartingOffset = (byte)(i - 1);
                    break;
                }
            }
        }

        #endregion

        #region IErrorDiffusion Interface

        public bool PreScan => false;

        public float[,] Matrix { get; }

        public byte MatrixHeight { get; }

        public byte MatrixWidth { get; }

        public byte StartingOffset { get; }

        public void Diffuse(float[,] gray, int x, int y, RectInt32 bounds, float threshold = 128)
        {
            var error = gray[x, y] - (gray[x,y] < threshold ? 0 : 255);
            int width = bounds.Width;
            int height = bounds.Height;
            for (int row = 0; row < MatrixHeight; row++)
            {
                int offsetY = y + row;
                for (int col = 0; col < MatrixWidth; col++)
                {
                    var coefficient = Matrix[row, col];
                    int offsetX = x + (col - StartingOffset);

                    if (coefficient != 0.0f && offsetX > 0 && offsetX < width && offsetY > 0 && offsetY < height)
                    {
                        gray[offsetX, offsetY] += error * coefficient;
                    }
                }
            }
        }

        #endregion
    }
}