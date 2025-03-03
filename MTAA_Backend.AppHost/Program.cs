using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sqlserver")
                 .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("mtaaDb");

var cache = builder.AddRedis("cache")
                   .WithLifetime(ContainerLifetime.Persistent);

var qdrant = builder.AddQdrant("qdrant")
                    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.MTAA_Backend_Api>("mtaa-backend")
       .WithReference(cache)
       .WaitFor(cache)
       .WithReference(db)
       .WaitFor(db)
       .WithReference(qdrant)
       .WaitFor(qdrant);

builder.AddProject<Projects.MTAA_Backend_MigrationService>("migrations")
       .WithReference(db)
       .WaitFor(db);

builder.Build().Run();
