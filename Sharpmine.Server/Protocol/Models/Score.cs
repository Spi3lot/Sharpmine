namespace Sharpmine.Server.Protocol.Models;

public record Score
{

    public required string Name { get; init; }

    public required string Objective { get; init; }

}