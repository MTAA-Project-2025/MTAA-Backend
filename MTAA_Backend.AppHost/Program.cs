using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

builder.AddProject<Projects.MTAA_Backend_Api>("mtaa-backend")
       .WithReference(cache);

builder.Build().Run();
