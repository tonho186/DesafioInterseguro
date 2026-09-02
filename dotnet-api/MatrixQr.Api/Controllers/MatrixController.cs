using MatrixQr.Api.Data;
using MatrixQr.Api.Models;
using MatrixQr.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MatrixQr.Api.Controllers
{
    [ApiController]
    [Route("api/v1/matrices")]
    public sealed class MatrixController(
    IMatrixQrService qrService,
    IJavaStatisticsClient statisticsClient,
    ApplicationDbContext dbContext)
    : ControllerBase
    {
        [HttpPost("qr")]
        [ProducesResponseType(typeof(QrResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<ActionResult<QrResponse>> CalculateQr(
            [FromBody] MatrixRequest request,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var qr = qrService.CalculateQr(request.Matrix);

                var statistics =
                    await statisticsClient.CalculateStatisticsAsync(
                        qr,
                        cancellationToken);

                stopwatch.Stop();

                var entity = new MatrixRequestEntity
                {
                    Id = Guid.NewGuid(),
                    Rows = request.Matrix.Length,
                    Columns = request.Matrix[0].Length,
                    CreatedAtUtc = DateTime.UtcNow,
                    ProcessingTimeMs = stopwatch.ElapsedMilliseconds
                };

                dbContext.MatrixRequests.Add(entity);
                await dbContext.SaveChangesAsync(cancellationToken);

                var response = new QrResponse(
                    entity.Id,
                    request.Matrix.Length,
                    request.Matrix[0].Length,
                    qr,
                    statistics);

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    new
                    {
                        error = "Java statistics service is unavailable.",
                        detail = ex.Message
                    });
            }
        }
    }
}
