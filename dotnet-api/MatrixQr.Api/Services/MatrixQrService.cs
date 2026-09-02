using MatrixQr.Api.Models;

namespace MatrixQr.Api.Services
{
    /// <summary>
    /// Calculates the QR decomposition of a rectangular matrix
    /// using Householder reflections.
    /// </summary>
    public sealed class MatrixQrService : IMatrixQrService
    {
        private const double Epsilon = 1e-12;

        public MatrixPair CalculateQr(double[][] matrix)
        {
            ValidateMatrix(matrix);

            int m = matrix.Length;
            int n = matrix[0].Length;

            if (m < n)
            {
                throw new ArgumentException(
                    "The matrix must have rows >= columns for this QR implementation.");
            }

            var r = ToRectangularArray(matrix);

            var q = IdentityMatrix(m);

            int iterations = Math.Min(m, n);

            for (int k = 0; k < iterations; k++)
            {
                var v = new double[m - k];

                for (int i = k; i < m; i++)
                {
                    v[i - k] = r[i, k];
                }

                double norm = EuclideanNorm(v);

                if (norm < Epsilon)
                {
                    continue;
                }

                // Stable sign choice.
                if (v[0] >= 0)
                {
                    norm = -norm;
                }

                v[0] -= norm;

                double vNorm = EuclideanNorm(v);

                if (vNorm < Epsilon)
                {
                    continue;
                }

                for (int i = 0; i < v.Length; i++)
                {
                    v[i] /= vNorm;
                }

                // R = H * R
                ApplyHouseholderToRight(v, r, k);

                // Q = Q * H
                ApplyHouseholderToLeft(q, v, k);
            }

            // Clean numerical noise below the diagonal.
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < Math.Min(i, n); j++)
                {
                    if (Math.Abs(r[i, j]) < Epsilon)
                    {
                        r[i, j] = 0;
                    }
                }
            }

            return new MatrixPair(
                ToJaggedArray(q),
                ToJaggedArray(r));
        }

        private static void ApplyHouseholderToRight(
            double[] v,
            double[,] matrix,
            int offset)
        {
            int m = matrix.GetLength(0);
            int n = matrix.GetLength(1);

            for (int j = offset; j < n; j++)
            {
                double dot = 0;

                for (int i = offset; i < m; i++)
                {
                    dot += v[i - offset] * matrix[i, j];
                }

                for (int i = offset; i < m; i++)
                {
                    matrix[i, j] -= 2 * v[i - offset] * dot;
                }
            }
        }

        private static void ApplyHouseholderToLeft(
            double[,] q,
            double[] v,
            int offset)
        {
            int m = q.GetLength(0);

            for (int row = 0; row < m; row++)
            {
                double dot = 0;

                for (int i = offset; i < m; i++)
                {
                    dot += q[row, i] * v[i - offset];
                }

                for (int i = offset; i < m; i++)
                {
                    q[row, i] -= 2 * dot * v[i - offset];
                }
            }
        }

        private static double EuclideanNorm(double[] vector)
        {
            double sum = 0;

            foreach (double value in vector)
            {
                sum += value * value;
            }

            return Math.Sqrt(sum);
        }

        private static double[,] IdentityMatrix(int size)
        {
            var result = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                result[i, i] = 1;
            }

            return result;
        }

        private static double[,] ToRectangularArray(double[][] source)
        {
            int rows = source.Length;
            int columns = source[0].Length;

            var result = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = source[i][j];
                }
            }

            return result;
        }

        private static double[][] ToJaggedArray(double[,] source)
        {
            int rows = source.GetLength(0);
            int columns = source.GetLength(1);

            var result = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new double[columns];

                for (int j = 0; j < columns; j++)
                {
                    result[i][j] = source[i, j];
                }
            }

            return result;
        }

        private static void ValidateMatrix(double[][]? matrix)
        {
            if (matrix is null || matrix.Length == 0)
            {
                throw new ArgumentException("Matrix cannot be empty.");
            }

            if (matrix[0] is null || matrix[0].Length == 0)
            {
                throw new ArgumentException(
                    "Matrix must contain at least one column.");
            }

            int columns = matrix[0].Length;

            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix[i] is null)
                {
                    throw new ArgumentException(
                        $"Row {i} cannot be null.");
                }

                if (matrix[i].Length != columns)
                {
                    throw new ArgumentException(
                        "Matrix must be rectangular.");
                }

                foreach (double value in matrix[i])
                {
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException(
                            "Matrix values must be finite numbers.");
                    }
                }
            }
        }
    }
}
