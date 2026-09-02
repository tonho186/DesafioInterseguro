namespace MatrixQr.Api.Models
{
    public sealed record QrResponse(
    Guid RequestId,
    int Rows,
    int Columns,
    MatrixPair Result,
    object? Statistics);
}

