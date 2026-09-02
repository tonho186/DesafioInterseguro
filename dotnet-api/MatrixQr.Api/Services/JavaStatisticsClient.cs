using MatrixQr.Api.Models;

namespace MatrixQr.Api.Services
{
    public sealed class JavaStatisticsClient(
    HttpClient httpClient) : IJavaStatisticsClient
    {
        public async Task<object> CalculateStatisticsAsync(
            MatrixPair matrices,
            CancellationToken cancellationToken)
        {
            using var response = await httpClient.PostAsJsonAsync(
                "/api/v1/statistics",
                matrices,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<object>(
                cancellationToken: cancellationToken);

            return result
                ?? throw new InvalidOperationException(
                    "Java API returned an empty response.");
        }
    }
}
