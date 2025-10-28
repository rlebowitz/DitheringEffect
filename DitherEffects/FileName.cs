using System;
using System.Drawing;

class FloydSteinbergColorDither
{
    //static void Main(string[] args)
    //{
    //    string inputPath = "input.png";   // Replace with your image path
    //    string outputPath = "output_color_dither.png";

    //    int levelsPerChannel = 4; // Try 2, 4, 8, or 16 for different effects

    //    Bitmap original = new Bitmap(inputPath);
    //    Bitmap dithered = ApplyColorDither(original, levelsPerChannel);

    //    dithered.Save(outputPath);
    //    Console.WriteLine("Color dithering complete. Saved to " + outputPath);
    //}

    //static Bitmap ApplyColorDither(Bitmap bmp, int levels)
    //{
    //    int width = bmp.Width;
    //    int height = bmp.Height;
    //    Bitmap result = new Bitmap(width, height);

    //    float step = 255f / (levels - 1);
    //    float[,] r = new float[width, height];
    //    float[,] g = new float[width, height];
    //    float[,] b = new float[width, height];

    //    // Copy original RGB values into float arrays
    //    for (int y = 0; y < height; y++)
    //        for (int x = 0; x < width; x++)
    //        {
    //            Color pixel = bmp.GetPixel(x, y);
    //            r[x, y] = pixel.R;
    //            g[x, y] = pixel.G;
    //            b[x, y] = pixel.B;
    //        }

    //    for (int y = 0; y < height; y++)
    //        for (int x = 0; x < width; x++)
    //        {
    //            // Quantize each channel
    //            float oldR = r[x, y];
    //            float oldG = g[x, y];
    //            float oldB = b[x, y];

    //            float newR = (float)Math.Round(oldR / step) * step;
    //            float newG = (float)Math.Round(oldG / step) * step;
    //            float newB = (float)Math.Round(oldB / step) * step;

    //            result.SetPixel(x, y, Color.FromArgb(
    //                ClampToByte(newR),
    //                ClampToByte(newG),
    //                ClampToByte(newB)));

    //            float errR = oldR - newR;
    //            float errG = oldG - newG;
    //            float errB = oldB - newB;

    //            // Distribute error to neighbors
    //            void Diffuse(int dx, int dy, float factor)
    //            {
    //                int nx = x + dx;
    //                int ny = y + dy;
    //                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
    //                {
    //                    r[nx, ny] += errR * factor;
    //                    g[nx, ny] += errG * factor;
    //                    b[nx, ny] += errB * factor;
    //                }
    //            }

    //            Diffuse(1, 0, 7 / 16f);
    //            Diffuse(-1, 1, 3 / 16f);
    //            Diffuse(0, 1, 5 / 16f);
    //            Diffuse(1, 1, 1 / 16f);
    //        }

    //    return result;
    //}

    static int ClampToByte(float value)
    {
        return Math.Max(0, Math.Min(255, (int)value));
    }
}
