using Asp.Versioning;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Api.Extensions;
using MTAA_Backend.Api.Middlewares;
using MTAA_Backend.Application.Services;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;
using MTAA_Backend.Application.Validators.Identity;
using MTAA_Backend.Application.MaperProfiles.Users;
using System.Runtime.CompilerServices;
using MTAA_Backend.Api.Configs;
using MTAA_Backend.Application.CQRS.Users.Identity.Queries;


public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //var connectionString = builder.Configuration.GetConnectionString("DbContextConnection") ?? throw new InvalidOperationException("Connection string 'DbContextConnection' not found.");

        ConfigurationManager configuration = builder.Configuration;


        builder.AddSqlServerClient(connectionName: "mtaaDb");
        builder.AddSqlServerDbContext<MTAA_BackendDbContext>(connectionName: "mtaaDb");
        builder.AddQdrantClient("qdrant");

        builder.Services.AddHostedService<DbMigrationJob>();

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.AddServiceDefaults();

        builder.AddRedisDistributedCache("cache");
        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddAutoMapper(typeof(UserMapperProfile).Assembly);
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(LogIn).Assembly));

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(typeof(LogInRequestValidator).Assembly);

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddScoped<ICodeGeneratorService, CodeGeneratorService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<ILanguageService, LanguageService>();
        builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAccountService, AccountService>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.ConfigureSwagger();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddLocalization();
        builder.Services.ConfigureJWT(configuration);

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}