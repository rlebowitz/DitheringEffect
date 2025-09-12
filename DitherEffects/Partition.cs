namespace Dithering
{
    public struct Partition
    {
        public int PixelCount { get; private set; }
        public System.Numerics.Vector3 PixelSum { get; private set; }
        public readonly System.Numerics.Vector3 PixelAverage { get => PixelSum / PixelCount; }

        public void AddPixel(System.Numerics.Vector3 pixel)
        {
            PixelCount++;
            PixelSum += pixel;
        }
    }
}

