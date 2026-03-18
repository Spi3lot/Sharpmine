using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Sharpmine.Server.Gui;
using Sharpmine.Server.Logging;
using Sharpmine.Server.Protocol;

namespace Sharpmine.Server;

public static class Program
{

    [STAThread]
    public static async Task Main(params string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        Console.WriteLine($"Current Environment: {builder.Environment.EnvironmentName}");

        bool nogui = builder.Configuration.GetValue<bool>("nogui");
        ushort port = builder.Configuration.GetValue<ushort?>("port") ?? 25565;
        var sink = new ListLogEventSink();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Sink(sink)
            .CreateLogger();

        builder.Services.AddSerilog();
        builder.Services.AddSingleton(sink);
        builder.Services.AddSingleton<ClientHandlerFactory>();
        builder.Services.AddSingleton<Form1>();
        builder.Services.AddSingleton<ServerFactory>();
        builder.Services.AddSingleton<ServerService>(sp => sp.GetRequiredService<ServerFactory>().Create(port));
        builder.Services.AddHostedService<ServerService>(sp => sp.GetRequiredService<ServerService>());

        using var host = builder.Build();

        if (nogui || args.Contains("--nogui", StringComparer.OrdinalIgnoreCase)
                  || args.Contains("/nogui", StringComparer.OrdinalIgnoreCase))
        {
            await host.RunAsync();
            return;
        }

        await host.StartAsync();
        ApplicationConfiguration.Initialize();
        Application.Run(host.Services.GetRequiredService<Form1>());
        await host.StopAsync();
    }

}
