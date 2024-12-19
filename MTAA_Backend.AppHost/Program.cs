var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MTAA_Backend>("mtaa-backend");

builder.Build().Run();
