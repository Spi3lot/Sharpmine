using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Sharpmine.Server.Configuration;
using Sharpmine.Server.Gui;
using Sharpmine.Server.Logging;
using Sharpmine.Server.Protocol;
using Sharpmine.Server.Security;

namespace Sharpmine.Server;

public static class Program
{

    [STAThread]
    public static async Task Main(params string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.AddServiceDefaults();

            bool nogui = builder.Configuration.GetValue<bool>("nogui");
            var sink = new ListLogEventSink();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Sink(sink)
                .CreateLogger();

            var propertiesConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddIniFile(ServerConstants.PropertiesFileName, optional: true, reloadOnChange: true)
                .Build();

            builder.Services.AddSerilog();
            builder.Services.AddSingleton(propertiesConfig.Get<ServerProperties>() ?? new ServerProperties());
            builder.Services.AddSingleton(sink);
            builder.Services.AddSingleton<Form1>();
            builder.Services.AddSingleton<PlayerAccessManager>();
            builder.Services.AddSingleton<ClientHandlerFactory>();
            builder.Services.AddSingleton<PacketDispatcher>();
            builder.Services.AddSingleton<ServerService>();
            builder.Services.AddSingleton<ServerCapacityManager>();
            builder.Services.AddHostedService<ServerService>(sp => sp.GetRequiredService<ServerService>());

            builder.Services.Scan(scan => scan
                .FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IPacketHandler<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

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
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application encountered a fatal error and crashed");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

}
