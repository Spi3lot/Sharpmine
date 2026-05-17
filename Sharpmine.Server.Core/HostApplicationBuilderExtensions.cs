using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Sharpmine.Server.Core.Configuration;
using Sharpmine.Server.Core.Domain;
using Sharpmine.Server.Core.Protocol;
using Sharpmine.Server.Core.Protocol.Handlers;
using Sharpmine.Server.Core.Protocol.Packets;
using Sharpmine.Server.Core.Security;

namespace Sharpmine.Server.Core;

public static class HostApplicationBuilderExtensions
{

    extension<TBuilder>(TBuilder builder) where TBuilder : IHostApplicationBuilder
    {

        public TBuilder AddCoreServices()
        {
            builder.Configuration.AddIniFile(
                ServerConstants.FileNames.Properties,
                optional: true,
                reloadOnChange: true);

            builder.Services.AddSerilog();
            builder.Services.AddSingleton(builder.Configuration.Get<ServerProperties>() ?? new ServerProperties());
            builder.Services.AddSingleton<RegistryCache>();
            builder.Services.AddSingleton<PlayerAccessManager>();
            builder.Services.AddSingleton<PacketReceiver>();
            builder.Services.AddSingleton<PacketDispatcher>();
            builder.Services.AddTransient<PacketSerializer>();
            builder.Services.AddTransient<PacketTransmitter>();
            builder.Services.AddSingleton<ServerCapacityManager>();
            builder.Services.AddSingleton<ServerService>();
            builder.Services.AddHostedService<ServerService>(sp => sp.GetRequiredService<ServerService>());

            builder.Services.Scan(scan => scan
                .FromAssemblyOf<ServerService>()
                .AddClasses(classes => classes.AssignableTo(typeof(IPacketHandler<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            return builder.AddFactoryServices();
        }

        public TBuilder AddFactoryServices()
        {
            builder.Services.AddSingleton<ClientHandlerFactory>();
            builder.Services.AddSingleton<TransmissionWorkerFactory>();
            builder.Services.AddSingleton<DispatchWorkerFactory>();
            return builder;
        }

    }

}
