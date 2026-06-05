using System.Text.Json.Serialization;

namespace Sharpmine.Domain.Providers.Int;

[JsonConverter(typeof(IntProviderConverter))]
public interface IIntProvider
{

    Identifier Type { get; }

    int Sample(Random random);

}
