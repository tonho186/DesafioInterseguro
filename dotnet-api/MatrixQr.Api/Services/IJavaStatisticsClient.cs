using MatrixQr.Api.Models;

namespace MatrixQr.Api.Services
{
    public interface IJavaStatisticsClient
    {
        Task<object> CalculateStatisticsAsync(
            MatrixPair matrices,
            CancellationToken cancellationToken);
    }
}
