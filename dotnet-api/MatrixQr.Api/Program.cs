using MatrixQr.Api.Data;
using MatrixQr.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IMatrixQrService, MatrixQrService>();

builder.Services.AddHttpClient<IJavaStatisticsClient, JavaStatisticsClient>(
    client =>
    {
        var baseUrl =
            builder.Configuration["JavaApi:BaseUrl"]
            ?? "http://java-api:8080";

        client.BaseAddress = new Uri(baseUrl);

        client.Timeout = TimeSpan.FromSeconds(10);
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/health", () =>
    Results.Ok(new
    {
        status = "UP",
        service = "dotnet-api"
    }));

app.Run();

public partial class Program;