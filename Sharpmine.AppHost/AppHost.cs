var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Sharpmine_Server>("sharpmine-server");

builder.Build().Run();
