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

            Matrix = matrix;
            MatrixWidth = (byte)(matrix.GetUpperBound(1) + 1);
            MatrixHeight = (byte)(matrix.GetUpperBound(0) + 1);
            Divisor = divisor;
            UseShifting = useShifting;

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

        public byte[,] Matrix { get; }

        public byte Divisor { get; }

        public byte MatrixHeight { get; }

        public byte MatrixWidth { get; }

        public byte StartingOffset { get; }

        public bool UseShifting { get; }

        public void Diffuse(RegionPtr<ColorBgra32> data, ColorBgra32 original, int x, int y, RectInt32 bounds)
        {
            var redError = original.R - data[x,y].R;
            var greenError = original.G - data[x,y].G;
            var blueError = original.B - data[x, y].B;
            int width = bounds.Width;
            int height = bounds.Height;
            for (int row = 0; row < MatrixHeight; row++)
            {
                
                int offsetY = y + row;
                for (int col = 0; col < MatrixWidth; col++)
                {
                    int coefficient = Matrix[row, col];
                    int offsetX = x + (col - StartingOffset);

                    if (coefficient != 0 && offsetX > 0 && offsetX < width && offsetY > 0 && offsetY < height)
                    {
                        ColorBgra32 offsetPixel = data[offsetX, offsetY];

                        // if the UseShifting property is set, then bit shift the values by the specified
                        // divisor as this is faster than integer division. Otherwise, use integer division
                        int newR;
                        int newG;
                        int newB;

                        if (UseShifting)
                        {
                            newR = redError * coefficient >> Divisor;
                            newG = greenError * coefficient >> Divisor;
                            newB = blueError * coefficient >> Divisor;
                        }
                        else
                        {
                            newR = redError * coefficient / Divisor;
                            newG = greenError * coefficient / Divisor;
                            newB = blueError * coefficient / Divisor;
                        }

                        byte r = (offsetPixel.R + newR).ToByte();
                        byte g = (offsetPixel.G + newG).ToByte();
                        byte b = (offsetPixel.B + newB).ToByte();

                        data[offsetX,offsetY] = ColorBgra32.FromBgra(b, g, r, 255);
                    }
                }
            }
        }

        #endregion
    }
}