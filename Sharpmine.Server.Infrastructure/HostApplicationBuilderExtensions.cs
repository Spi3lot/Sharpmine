using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Sharpmine.Domain;
using Sharpmine.Domain.Registries;
using Sharpmine.Domain.Tags;
using Sharpmine.Server.Infrastructure.Configuration;
using Sharpmine.Server.Infrastructure.Protocol;
using Sharpmine.Server.Infrastructure.Protocol.Handlers;
using Sharpmine.Server.Infrastructure.Protocol.Packets;
using Sharpmine.Server.Infrastructure.Security;

namespace Sharpmine.Server.Infrastructure;

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

            builder.AddDomainServices();
            builder.AddRegistryServices();
            builder.AddTagServices();
            builder.Services.AddSerilog();
            builder.Services.AddSingleton(builder.Configuration.Get<ServerProperties>() ?? new ServerProperties());
            builder.Services.AddSingleton<IRegistryLoader, DiskRegistryLoader>();
            builder.Services.AddSingleton<NetworkRegistryCache>();
            builder.Services.AddSingleton<PlayerAccessManager>();
            builder.Services.AddSingleton<PacketReceiver>();
            builder.Services.AddSingleton<PacketDispatcher>();
            builder.Services.AddTransient<PacketSerializer>();
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

        public TBuilder AddRegistryServices()
        {
            builder.Services.AddSingleton<IRegistryLoader, DiskRegistryLoader>();
            builder.Services.AddSingleton<NetworkRegistryCache>();
            return builder;
        }

        public TBuilder AddTagServices()
        {
            builder.Services.AddSingleton<ITagLoader, DiskTagLoader>();
            builder.Services.AddSingleton<NetworkTagCache>();
            return builder;
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
