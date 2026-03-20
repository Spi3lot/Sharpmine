var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Sharpmine_Server>("sharpmine-server");

await builder.Build().RunAsync();
