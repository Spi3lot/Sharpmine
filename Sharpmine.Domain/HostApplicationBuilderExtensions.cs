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
                var provider = sp.GetRequiredService<IRegistryProvider>();
                return new RegistryCache(provider.Get());
            });

            builder.Services.AddSingleton(sp =>
            {
                var provider = sp.GetRequiredService<ITagProvider>();
                return new TagCache(provider.Get());
            });

            return builder;
        }

    }

}
