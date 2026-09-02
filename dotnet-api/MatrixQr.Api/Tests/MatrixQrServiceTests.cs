using MatrixQr.Api.Services;
using Xunit;

namespace MatrixQr.Api.Tests
{
    public class MatrixQrServiceTests
    {
        private readonly MatrixQrService _service = new();

        [Fact]
        public void CalculateQr_ShouldProduceOrthogonalQ()
        {
            var matrix = new[]
            {
            new[] { 1.0, 2.0 },
            new[] { 3.0, 4.0 },
            new[] { 5.0, 6.0 }
        };

            var result = _service.CalculateQr(matrix);

            var q = result.Q;

            for (int i = 0; i < q[0].Length; i++)
            {
                for (int j = 0; j < q[0].Length; j++)
                {
                    double dot = 0;

                    for (int k = 0; k < q.Length; k++)
                    {
                        dot += q[k][i] * q[k][j];
                    }

                    if (i == j)
                    {
                        Assert.InRange(
                            Math.Abs(dot - 1),
                            0,
                            1e-9);
                    }
                    else
                    {
                        Assert.InRange(
                            Math.Abs(dot),
                            0,
                            1e-9);
                    }
                }
            }
        }

        [Fact]
        public void CalculateQr_ShouldRejectNonRectangularMatrix()
        {
            var matrix = new[]
            {
            new[] { 1.0, 2.0 },
            new[] { 3.0 }
        };

            Assert.Throws<ArgumentException>(
                () => _service.CalculateQr(matrix));
        }

        [Fact]
        public void CalculateQr_ShouldRejectWideMatrix()
        {
            var matrix = new[]
            {
            new[] { 1.0, 2.0, 3.0 }
        };

            Assert.Throws<ArgumentException>(
                () => _service.CalculateQr(matrix));
        }
    }
}
