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
using MTAA_Backend.Application.CQRS.Users.Identity.Queries;
using Hangfire;
using Hangfire.SqlServer;
using MTAA_Backend.Application.Repositories;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Application.Services.RecommendationSystem.RecommendationFeedServices;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Application.Services.RecommendationSystem;
using Microsoft.Extensions.DependencyInjection;
using Betalgo.Ranul.OpenAI.Extensions;
using MTAA_Backend.Domain.Entities.Users;
using Hangfire.PostgreSql;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Application.Services.Locations;
using MTAA_Backend.MigrationService;
using Microsoft.Extensions.Options;
using System.Reflection.PortableExecutable;
using Microsoft.Identity.Client.Extensions.Msal;


public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("mtaaDb") ?? throw new InvalidOperationException("Connection string 'mtaaDb' not found.");

        ConfigurationManager configuration = builder.Configuration;


        builder.AddNpgsqlDataSource(connectionName: "mtaaDb");
        builder.AddNpgsqlDbContext<MTAA_BackendDbContext>(connectionName: "mtaaDb", configureDbContextOptions: configure =>
        {
            configure.UseNpgsql(opt =>
            {
                opt.UseNetTopologySuite();
                opt.EnableRetryOnFailure();
            });

        });
        builder.AddQdrantClient("qdrant");
        
        builder.Services.AddOpenAIService();

        //builder.Services.AddHostedService<DbMigrationJob>();

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.AddServiceDefaults();

        builder.AddRedisDistributedCache("cache");

        //builder.Services.AddHostedService<Worker>();

        builder.Services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString)));

        builder.Services.AddHangfireServer();

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

        builder.Services.AddScoped<IVectorDatabaseRepository, VectorDatabaseRepository>();
        builder.Services.AddScoped<IPostsFromFollowersRecommendationFeedService, PostsFromFollowersRecommendationFeedService>();
        builder.Services.AddScoped<IPostsFromGlobalPopularityRecommendationFeedService, PostsFromGlobalPopularityRecommendationFeedService>();
        builder.Services.AddScoped<IPostsFromPreferencesRecommendationFeedService, PostsFromPreferencesRecommendationFeedService>();
        builder.Services.AddScoped<IEmbeddingsService, EmbeddingsService>();
        builder.Services.AddScoped<IPostsConfigureRecommendationsService, PostsConfigureRecommendationsService>();
        builder.Services.AddScoped<IRecommendationItemsService, RecommendationItemsService>();
        builder.Services.AddScoped<IVersionItemService, VersionItemService>();

        builder.Services.AddScoped<INormalizeLocationService, NormalizeLocationService>();
        builder.Services.AddScoped<ILocationService, LocationService>();

        builder.Services.AddSingleton<IMLNetService, MLNetService>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.ConfigureSwagger();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddLocalization();
        builder.Services.ConfigureJWT(configuration);

        var app = builder.Build();

        Task.Delay(10000);
        using var scope = app.Services.CreateScope();
        var dbcontext = scope.ServiceProvider.GetRequiredService<MTAA_BackendDbContext>();
        //dbcontext.Database.EnsureDeletedAsync().Wait();
        dbcontext.Database.MigrateAsync().Wait();


        app.MapDefaultEndpoints();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseHangfireDashboard();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
            app.UseReDoc();
        }

        app.UseHttpsRedirection();

        app.ConfigureLocalization();
        app.ConfigureQdrant().Wait();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.ConfigureHangfireJobs();
        app.ConfigureAdminUser().Wait();

        app.Run();
    }
}