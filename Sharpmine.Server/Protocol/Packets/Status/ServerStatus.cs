using System.Text;

namespace Sharpmine.Server.Protocol.Packets.Status;

public record ServerStatus(
    StatusVersion Version,
    StatusPlayers? Players = null,
    StatusDescription? Description = null,
    string? Favicon = null,
    bool EnforcesSecureChat = false)
{

    public override string ToString()
    {
        var builder = new StringBuilder();
        (this with { Favicon = null }).PrintMembers(builder);
        return builder.ToString();
    }

}

public record StatusVersion(string Name, int? Protocol = null);

public record StatusPlayers(int Max, int Online, List<StatusPlayer>? Sample = null)
{

    public override string ToString()
    {
        var builder = new StringBuilder();
        (this with { Sample = null }).PrintMembers(builder);
        return builder.ToString();
    }

}

public record StatusPlayer(string Name, Guid Id);

public record StatusDescription(string Text);
