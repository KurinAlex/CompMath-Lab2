using System.Globalization;

namespace CompMath_Lab2
{
    public struct Matrix
    {
        public Matrix(double[][] matrix, bool checkDimentions = true)
        {
            if (checkDimentions && matrix.Any(row => row.Length != matrix[0].Length))
            {
                throw new ArgumentException("Matrix has different row dimentions");
            }

            _matrix = MatrixHelper.Copy(matrix);
            _height = matrix.Length;
            _width = matrix[0].Length;
        }
        public Matrix(string filePath)
            : this(File.ReadAllLines(filePath)
                .Select(line =>
                    line
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => double.Parse(n, CultureInfo.InvariantCulture))
                    .ToArray()
                ).ToArray())
        { }

        public double this[int i, int j] => _matrix[i][j];

        public Matrix GetInverse()
        {
            return Solve(this, GetIdentity(_height)).X;
        }
        public string ToString(bool exponential = false)
        {
            return MatrixHelper.ToString(_matrix, exponential);
        }

        public static Matrix GetIdentity(int n)
        {
            return new(
                Enumerable.Range(0, n)
                .Select(k =>
                    Enumerable.Repeat(0, k)
                    .Append(1)
                    .Concat(Enumerable.Repeat(0, n - k - 1))
                    .Select(i => (double)i)
                    .ToArray()
                ).ToArray(),
                false);
        }
        public static (Matrix X, double det) Solve(Matrix A, Matrix B, Writer? writer = null)
        {
            int m = A._height;

            if (m != A._width)
            {
                throw new ArgumentException("Matrix is not square");
            }

            if (m != B._height)
            {
                throw new ArgumentException("Matrixes have different number of rows");
            }

            double[][] matrix = A._matrix
                .Zip(B._matrix)
                .Select(t => t.First.Concat(t.Second).ToArray())
                .ToArray();
            int n = matrix[0].Length;
            double det = 1.0;

            void Write(string message)
            {
                writer?.WriteLine(message);
                writer?.WriteLine(MatrixHelper.ToString(matrix));
                writer?.WriteDivider();
            }

            Write("Input equation:");

            for (int k = 0; k < m; k++)
            {
                int kMax = matrix
                    .Skip(k)
                    .Select((row, i) => (row[0], i))
                    .MaxBy(t => Math.Abs(t.Item1))
                    .Item2 + k;

                if (matrix[kMax][k] == 0.0)
                {
                    throw new ArgumentException("Matrix is singular");
                }

                if (k != kMax)
                {
                    (matrix[k], matrix[kMax]) = (matrix[kMax], matrix[k]);
                    det *= -1;
                }

                double f = matrix[k][k];
                for (int j = k; j < n; j++)
                {
                    matrix[k][j] /= f;
                }
                det *= f;

                for (int i = 0; i < m; i++)
                {
                    if (i == k)
                    {
                        continue;
                    }

                    f = matrix[i][k] / matrix[k][k];
                    for (int j = k; j < n; j++)
                    {
                        matrix[i][j] -= matrix[k][j] * f;
                    }
                }
                Write($"{k + 1} iteration:");
            }
            return (new(matrix.Select(row => row.Skip(m).ToArray()).ToArray()), det);
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            int height = left._height, width = left._width;

            if (height != right._height || width != right._width)
            {
                throw new ArgumentException("Matrixes dimentions are not equal");
            }

            double[][] res = new double[height][];

            for (int i = 0; i < height; i++)
            {
                res[i] = new double[width];
                for (int j = 0; j < width; j++)
                {
                    res[i][j] = left[i, j] - right[i, j];
                }
            }

            return new Matrix(res, false);
        }
        public static Matrix operator *(Matrix left, Matrix right)
        {
            int aWidth = left._width;

            if (aWidth != right._height)
            {
                throw new ArgumentException("Matrixes have wrong dimentions for multiplicaion");
            }

            int aHeight = left._height;
            int bWidth = right._width;
            double[][] res = new double[aHeight][];

            for (int i = 0; i < aHeight; i++)
            {
                res[i] = new double[bWidth];
                for (int j = 0; j < bWidth; j++)
                {
                    for (int k = 0; k < aWidth; k++)
                    {
                        res[i][j] += left[i, k] * right[k, j];
                    }
                }
            }

            return new Matrix(res, false);
        }

        private readonly double[][] _matrix;
        private readonly int _height;
        private readonly int _width;
    }
}
