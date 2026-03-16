namespace Sharpmine.Server.Protocol.Packets.Status;

public readonly record struct ServerStatus
{

    public StatusVersion Version { get; init; }

    public StatusPlayers Players { get; init; }

    public StatusDescription Description { get; init; }

    public string Favicon { get; init; }

    public bool EnforcesSecureChat { get; init; }

    public readonly record struct StatusVersion
    {

        public string Name { get; init; }

        public int Protocol { get; init; }

    }

    public readonly record struct StatusPlayers(int Max, int Online)
    {

        public List<Player> Sample { get; init; }

        public readonly record struct Player(string Name, Guid Uuid);

    }

    public readonly record struct StatusDescription(string Text);

}