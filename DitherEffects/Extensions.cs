#nullable disable

using System;

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
    }
}
