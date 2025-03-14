using Aspire.Hosting;
using FluentAssertions.Common;
using Google.Protobuf.WellKnownTypes;
using IntegrationTests.Config;
using IntegrationTests.Extensions;
using IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions.AssemblyFixture;

[assembly: TestFramework(AssemblyFixtureFramework.TypeName, AssemblyFixtureFramework.AssemblyName)]
namespace IntegrationTests.Fixtures
{
    public class ApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly DistributedApplication _app;
        private readonly IResourceBuilder<Aspire.Hosting.ApplicationModel.SqlServerServerResource> _sqlServer;
        private readonly IResourceBuilder<Aspire.Hosting.ApplicationModel.SqlServerDatabaseResource> _db;
        private readonly IResourceBuilder<Aspire.Hosting.ApplicationModel.RedisResource> _redis;
        private readonly IResourceBuilder<Aspire.Hosting.ApplicationModel.QdrantServerResource> _qdrant;
        private string? _sqlServerConnectionString;
        private string? _redisConnectionString;
        private string? _qdrantConnectionString;

        /**
         * Constructor for ApiFixture.
         * Initializes the DistributedApplicationOptions and sets up the SqlServer server resource.
         * Taken from the tutorial to Aspire.Hosting, but highly modified.
         */
        public ApiFixture()
        {
            var options = new DistributedApplicationOptions
            {
                AssemblyName = typeof(ApiFixture).Assembly.FullName,
                DisableDashboard = true
            };
            var builder = DistributedApplication.CreateBuilder(options);

            _sqlServer = builder.AddSqlServer("mtaaDb");

            builder.AddProject<MTAA_Backend_MigrationService>("migrations")
                .WithReference(_sqlServer)
                .WaitFor(_sqlServer);

            _redis = builder.AddRedis("cache");

            _qdrant = builder.AddQdrant("qdrant");

            builder.Services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            _app = builder.Build();
        }

        /**
         * Creates and configures the host for the application.
         * Adds the SqlServer connection string to the host configuration.
         * Ensures the database is created before returning the host.
         *
         * @param builder The IHostBuilder instance.
         * @return The configured IHost instance.
         */
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ConnectionStrings:mtaaDb", _sqlServerConnectionString },
                    { "ConnectionStrings:cache", _redisConnectionString },
                    { "ConnectionStrings:qdrant", _qdrantConnectionString },
                });
            });

            return base.CreateHost(builder);
        }

        /**
         * Disposes the resources used by the fixture asynchronously.
         * Stops the application host and disposes of it.
         */
        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();
            await _app.StopAsync();
            await _app.DisposeAsync();
        }

        /**
         * Initializes the fixture asynchronously.
         * Starts the application host and waits for the SqlServer resource to be in the running state.
         * Retrieve the SqlServer connection string.
         */
        public async Task InitializeAsync()
        {
            await _app.StartAsync();
            await _app.WaitForResourcesAsync();
            _sqlServerConnectionString = await _sqlServer.Resource.GetConnectionStringAsync();

            _redisConnectionString = await _redis.Resource.GetConnectionStringAsync();
            _qdrantConnectionString = await _qdrant.Resource.ConnectionStringExpression.GetValueAsync(CancellationToken.None);

            // Ensure that the SqlServer database is fully initialized before proceeding.
            // And Db migration is rant successfully.
            // This is crucial, especially in CI/CD environments, to prevent tests from failing due to timing issues.
            await Task.Delay(TimeSpan.FromSeconds(40));
        }
    }
}
