#nullable disable

using PaintDotNet.Imaging;
using System;
using System.Drawing;

namespace Dithering
{
    public static class Extensions
    {
        public static byte ToByte(this int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 255)
            {
                value = 255;
            }

            return (byte)value;
        }

        public static byte ToByte(this float value)
        {
            return value < 0.0f ? (byte)0 : value > 255.0f ? (byte)255 : Convert.ToByte(value);
        }

        public static byte ToByte(this double value)
        {
            return value < 0.0 ? (byte)0 : value > 255.0 ? (byte)255 : Convert.ToByte(value);
        }

        public static float ToGray(this ColorBgra32 color, LuminescenceWeights weights)
        {
            return (color.R * weights.RWeight) + (color.G * weights.GWeight) + (color.B * weights.BWeight);
        }

        public static ColorBgra32 ToColorBgra32(this float gray, float threshold)
        {
            return gray >= threshold ? ColorBgra32.FromBgra(255, 255, 255, 255) : ColorBgra32.FromBgra(0, 0, 0, 255);
        }

        public static int ClampToByte(float value)
        {
            return Math.Max(0, Math.Min(255, (int)value));
        }

        public static int ClampToByte(double value)
        {
            return Math.Max(0, Math.Min(255, (int)value));
        }
    }
}
