var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Sharpmine_Server_Console>("sharpmine-server-console");

await builder.Build().RunAsync();
