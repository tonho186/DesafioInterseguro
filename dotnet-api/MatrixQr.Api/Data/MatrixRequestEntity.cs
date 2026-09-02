namespace MatrixQr.Api.Data
{
    public sealed class MatrixRequestEntity
    {
        public Guid Id { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public long ProcessingTimeMs { get; set; }
    }
}
