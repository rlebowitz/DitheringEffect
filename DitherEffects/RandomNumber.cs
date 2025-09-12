using PaintDotNet.Rendering;
using System.Runtime.CompilerServices;

namespace Dithering
{
    internal static class RandomNumber
    {
        public static uint InitializeSeed(uint iSeed, float x, float y)
        {
            return CombineHashCodes(
                iSeed,
                CombineHashCodes(
                    Hash(Unsafe.As<float, uint>(ref x)),
                    Hash(Unsafe.As<float, uint>(ref y))));
        }

        public static uint InitializeSeed(uint instSeed, Point2Int32 scenePos)
        {
            return CombineHashCodes(
                instSeed,
                CombineHashCodes(
                    Hash(unchecked((uint)scenePos.X)),
                    Hash(unchecked((uint)scenePos.Y))));
        }

        public static uint Hash(uint input)
        {
            uint state = input * 747796405u + 2891336453u;
            uint word = (state >> (int)((state >> 28) + 4) ^ state) * 277803737u;
            return word >> 22 ^ word;
        }

        public static float NextFloat(ref uint seed)
        {
            seed = Hash(seed);
            return (seed >> 8) * 5.96046448E-08f;
        }

        public static int NextInt32(ref uint seed)
        {
            seed = Hash(seed);
            return unchecked((int)seed);
        }

        public static int NextInt32(ref uint seed, int maxValue)
        {
            seed = Hash(seed);
            return unchecked((int)(seed & 0x80000000) % maxValue);
        }

        public static int Next(ref uint seed)
        {
            seed = Hash(seed);
            return unchecked((int)seed);
        }

        public static int Next(ref uint seed, int maxValue)
        {
            seed = Hash(seed);
            return unchecked((int)(seed & 0x80000000) % maxValue);
        }

        public static byte NextByte(ref uint seed)
        {
            seed = Hash(seed);
            return (byte)(seed & 0xFF);
        }

        private static uint CombineHashCodes(uint hash1, uint hash2)
        {
            uint result = hash1;
            result = (result << 5) + result ^ hash2;
            return result;
        }
    }

}
