namespace Dithering
{
    public class Algorithm(double[,] matrix, int matrixOffset)
    {
        public double[,] Matrix { get; private set; } = matrix;
        public int MatrixOffset { get; private set; } = matrixOffset;
        public int MatrixWidth { get; private set; } = matrix.GetLength(1);
        public int MatrixHeight { get; private set; } = matrix.GetLength(0);
    }
}
