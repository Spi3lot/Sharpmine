using Sharpmine.Server.Protocol.Packets;

namespace Sharpmine.Server.Protocol;

public class ProtocolException(string message) : Exception(message)
{

    public static ProtocolException InvalidResult(ProtocolResult result) => new("Invalid protocol result: " + result);

}
