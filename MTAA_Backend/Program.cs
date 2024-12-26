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
