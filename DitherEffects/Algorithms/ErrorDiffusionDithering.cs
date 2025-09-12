/* Even more algorithms for dithering images using C#
 * https://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using PaintDotNet;
using PaintDotNet.Imaging;
using PaintDotNet.Rendering;
using System;

namespace Dithering.Algorithms
{
    public abstract class ErrorDiffusionDithering : IErrorDiffusion
    {
        #region Constants

        private readonly byte _divisor;

        private readonly byte[,] _matrix;

        private readonly byte _matrixHeight;

        private readonly byte _matrixWidth;

        private readonly byte _startingOffset;

        private readonly bool _useShifting;

        #endregion

        #region Constructors

        protected ErrorDiffusionDithering(byte[,] matrix, byte divisor, bool useShifting)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            if (matrix.Length == 0)
            {
                throw new ArgumentException("Matrix is empty.", nameof(matrix));
            }

            _matrix = matrix;
            _matrixWidth = (byte)(matrix.GetUpperBound(1) + 1);
            _matrixHeight = (byte)(matrix.GetUpperBound(0) + 1);
            _divisor = divisor;
            _useShifting = useShifting;

            for (int i = 0; i < _matrixWidth; i++)
            {
                if (matrix[0, i] != 0)
                {
                    _startingOffset = (byte)(i - 1);
                    break;
                }
            }
        }

        #endregion

        #region IErrorDiffusion Interface

        public bool PreScan => false;

        public void Diffuse(RegionPtr<ColorBgra32> data, ColorBgra32 original, ColorBgra32 transformed, int x, int y, RectInt32 bounds)
        {
            int redError = original.R - transformed.R;
            int greenError = original.G - transformed.G;
            int blueError = original.B - transformed.B;
            int width = bounds.Width;
            int height = bounds.Height;
            for (int row = 0; row < _matrixHeight; row++)
            {
                
                int offsetY = y + row;
                for (int col = 0; col < _matrixWidth; col++)
                {
                    int coefficient = _matrix[row, col];
                    int offsetX = x + (col - _startingOffset);

                    if (coefficient != 0 && offsetX > 0 && offsetX < width && offsetY > 0 && offsetY < height)
                    {
                        ColorBgra32 offsetPixel = data[offsetX, offsetY];

                        // if the UseShifting property is set, then bit shift the values by the specified
                        // divisor as this is faster than integer division. Otherwise, use integer division
                        int newR;
                        int newG;
                        int newB;

                        if (_useShifting)
                        {
                            newR = redError * coefficient >> _divisor;
                            newG = greenError * coefficient >> _divisor;
                            newB = blueError * coefficient >> _divisor;
                        }
                        else
                        {
                            newR = redError * coefficient / _divisor;
                            newG = greenError * coefficient / _divisor;
                            newB = blueError * coefficient / _divisor;
                        }

                        byte r = (offsetPixel.R + newR).ToByte();
                        byte g = (offsetPixel.G + newG).ToByte();
                        byte b = (offsetPixel.B + newB).ToByte();

                        data[offsetX,offsetY] = ColorBgra32.FromBgra(b, g, r, offsetPixel.A);
                    }
                }
            }
        }

        #endregion
    }
}