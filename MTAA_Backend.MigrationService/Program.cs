using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTAA_Backend.Infrastructure;
using MTAA_Backend.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<MTAA_BackendDbContext>("mtaaDb");

var host = builder.Build();
host.Run();
