using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sqlserver")
                 .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("mtaaDb");

var cache = builder.AddRedis("cache");

builder.AddProject<Projects.MTAA_Backend_Api>("mtaa-backend")
       .WithReference(cache)
       .WithReference(db)
       .WaitFor(db);

builder.AddProject<Projects.MTAA_Backend_MigrationService>("migrations")
       .WithReference(db)
       .WaitFor(db);

builder.Build().Run();
