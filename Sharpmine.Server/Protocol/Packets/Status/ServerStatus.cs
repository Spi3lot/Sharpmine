namespace Sharpmine.Server.Protocol.Packets.Status;

public record ServerStatus
{

    public required StatusVersion Version { get; init; }

    public StatusPlayers? Players { get; init; }

    public StatusDescription? Description { get; init; }

    public string? Favicon { get; init; }

    public bool EnforcesSecureChat { get; init; }

    public record StatusVersion(string Name, int Protocol);

    public record StatusPlayers(int Max, int Online)
    {

        public List<Player> Sample { get; init; } = [];

        public record Player(string Name, Guid Id);

    }

    public record StatusDescription(string Text);

}
