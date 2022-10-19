using CompMath_Lab2;

namespace Program
{
    public class Program
    {
        const string directoryPath = "D:\\Sources\\University\\2 course\\CompMath\\CompMath-Lab2";
        const string inputFolderName = "Input";
        const string matrixAFileName = "A.txt";
        const string matrixBFileName = "B.txt";
        const string outputFileName = "Result.txt";

        static readonly string matrixAFilePath = Path.Combine(directoryPath, inputFolderName, matrixAFileName);
        static readonly string matrixBFilePath = Path.Combine(directoryPath, inputFolderName, matrixBFileName);

        static void Main(string[] args)
        {
            using (StreamWriter fileWriter = new(outputFileName))
            {
                Writer writer = new(fileWriter);

                try
                {
                    writer.WriteLine("A:");
                    Matrix A = new(matrixAFilePath);
                    writer.WriteLine(Matrix.MatrixConverter.ToString(A));
                    writer.WriteDivider();

                    writer.WriteLine("B:");
                    Matrix B = new(matrixBFilePath);
                    writer.WriteLine(Matrix.MatrixConverter.ToString(B));
                    writer.WriteDivider();

                    writer.WriteLine("A * X = B");
                    writer.WriteDivider();

                    (Matrix X, double det) = Matrix.Solve(A, B, writer);

                    writer.WriteLine("X:");
                    writer.WriteLine(Matrix.MatrixConverter.ToString(X));
                    writer.WriteDivider();

                    writer.WriteLine("r = B - A * X:");
                    Matrix r = B - A * X;
                    writer.WriteLine(Matrix.MatrixConverter.ToString(r, true));
                    writer.WriteDivider();

                    writer.WriteLine("A^-1:");
                    Matrix A1 = A.GetInverse();
                    writer.WriteLine(Matrix.MatrixConverter.ToString(A1));
                    writer.WriteDivider();

                    writer.WriteLine($"det A = {det}");
                    writer.WriteDivider();

                    writer.WriteLine($"A * A^-1:");
                    Matrix E = A * A1;
                    writer.WriteLine(Matrix.MatrixConverter.ToString(E, true));
                    writer.WriteDivider();
                }
                catch (Exception ex)
                {
                    writer.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.ReadKey();
        }
    }
}