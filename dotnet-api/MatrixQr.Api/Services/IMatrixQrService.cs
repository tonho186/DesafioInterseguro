using MatrixQr.Api.Models;

namespace MatrixQr.Api.Services
{
    public interface IMatrixQrService
    {
        MatrixPair CalculateQr(double[][] matrix);
    }
}
