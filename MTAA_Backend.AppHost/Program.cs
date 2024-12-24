var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MTAA_Backend_Api>("mtaa-backend");

builder.Build().Run();
