using Microsoft.EntityFrameworkCore;

namespace MatrixQr.Api.Data
{
    public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
    {
        public DbSet<MatrixRequestEntity> MatrixRequests =>
            Set<MatrixRequestEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatrixRequestEntity>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Rows)
                    .IsRequired();

                entity.Property(x => x.Columns)
                    .IsRequired();

                entity.Property(x => x.CreatedAtUtc)
                    .IsRequired();

                entity.Property(x => x.ProcessingTimeMs)
                    .IsRequired();
            });
        }
    }
}
