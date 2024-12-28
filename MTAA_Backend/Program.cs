using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MTAA_Backend.Api.Extensions;
using MTAA_Backend.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ConfigureSwagger();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddLocalization();


var connectionString = builder.Configuration.GetConnectionString("LocalDbContextConnection") ?? throw new InvalidOperationException("Connection string 'LocalDbContextConnection' not found.");


ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<MTAA_BackendDbContext>(x => x.UseSqlServer(connectionString, builder =>
{
    builder.EnableRetryOnFailure(1, TimeSpan.FromSeconds(5), null);
}));


var app = builder.Build();

app.MapDefaultEndpoints();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.ConfigureLocalization();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
