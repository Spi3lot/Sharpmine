using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sharpmine.Domain.Registries;
using Sharpmine.Domain.Tags;

namespace Sharpmine.Domain;

public static class HostApplicationBuilderExtensions
{

    extension<TBuilder>(TBuilder builder) where TBuilder : IHostApplicationBuilder
    {

        public TBuilder AddDomainServices()
        {
            builder.Services.AddSingleton(sp =>
            {
                var loader = sp.GetRequiredService<IRegistryLoader>();
                return new RegistryCache(loader.Load());
            });

            builder.Services.AddSingleton(sp =>
            {
                var loader = sp.GetRequiredService<ITagLoader>();
                return new TagCache(loader.Load());
            });

            return builder;
        }

    }

}
