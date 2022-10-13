using System.Globalization;

namespace CompMath_Lab2
{
    public class MatrixHelper
    {
        public static double[][] Copy(double[][] source)
        {
            int height = source.Length;
            int width = source[0].Length;
            double[][] result = new double[height][];

            for(int i = 0; i < height; i++)
            {
                result[i] = new double[width];
                source[i].CopyTo(result[i], 0);
            }
            return result;
        }

        public static string ToString(double[][] matrix, bool exponential = false)
        {
            string format = exponential ? _exponentialFormat : _fixedPointFormat;
            return string.Join(
                Environment.NewLine,
                matrix.Select(row =>
                    string.Join(' ', row.Select(n => string.Format(CultureInfo.InvariantCulture, format, n)))
                )
            );
        }

        private const int _precision = 6;
        private const int _exponentialAlignment = _precision + 9;
        private const int _fixedPoinAlignment = _precision + 4;

        private static readonly string _exponentialFormat = $"{{0,{_exponentialAlignment}:E{_precision}}}";
        private static readonly string _fixedPointFormat = $"{{0,{_fixedPoinAlignment}:F{_precision}}}";
    }
}
