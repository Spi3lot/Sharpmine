using System.Text.Json.Nodes;

using Sharpmine.Server.Packets.Clientbound;

namespace Sharpmine.Server.Packets.Serverbound;

[Packet(0x00)]
public class HandshakePacket : IServerboundPacket
{

    public int ProtocolVersion { get; set; }

    public string ServerAddress { get; set; }

    public ushort ServerPort { get; set; }

    public Intent Intent { get; set; }

    public void Deserialize(BinaryReader reader)
    {
        ProtocolVersion = reader.Read7BitEncodedInt();
        ServerAddress = reader.ReadString();
        ServerPort = reader.ReadUInt16();
        Intent = (Intent) reader.Read7BitEncodedInt();
    }

    public void Process(ClientHandler handler, BinaryReader reader, BinaryWriter writer)
    {
        Console.WriteLine($"{ServerAddress}:{ServerPort} (protocol: {ProtocolVersion}, intent: {Intent})");
        handler.ConnectionState = (Intent == Intent.Status) ? ConnectionState.Status : ConnectionState.Login;

        var status = new JsonObject
        {
            {
                "version", new JsonObject
                {
                    { "name", "1.21.1" },
                    { "protocol", 772 /*_server.ProtocolVersion*/ },
                }
            },
            {
                "players", new JsonObject
                {
                    { "max", 1337 /*_server.Config.MaxPlayerCount*/ },
                    { "online", 2 /*_server.ActiveConnectionCount*/ },
                    {
                        "sample", new JsonArray(
                            new JsonObject
                            {
                                { "name", "Deswegen" },
                                { "id", "7e5abf92-0bf6-4b21-8c12-ad9a32720f3b" },
                            },
                            new JsonObject
                            {
                                { "name", "Yes_Mc" },
                                { "id", "ae2f02a6-001e-46b6-a659-5e016cf6e8e5" },
                            }
                        )
                    },
                }
            },
            {
                "description", new JsonObject
                {
                    { "text", "Heya" }
                }
            },
            {
                "favicon",
                "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANCSURBVEiJtZZPbBtFFMZ/M7ubXdtdb1xSFyeilBapySVU8h8OoFaooFSqiihIVIpQBKci6KEg9Q6H9kovIHoCIVQJJCKE1ENFjnAgcaSGC6rEnxBwA04Tx43t2FnvDAfjkNibxgHxnWb2e/u992bee7tCa00YFsffekFY+nUzFtjW0LrvjRXrCDIAaPLlW0nHL0SsZtVoaF98mLrx3pdhOqLtYPHChahZcYYO7KvPFxvRl5XPp1sN3adWiD1ZAqD6XYK1b/dvE5IWryTt2udLFedwc1+9kLp+vbbpoDh+6TklxBeAi9TL0taeWpdmZzQDry0AcO+jQ12RyohqqoYoo8RDwJrU+qXkjWtfi8Xxt58BdQuwQs9qC/afLwCw8tnQbqYAPsgxE1S6F3EAIXux2oQFKm0ihMsOF71dHYx+f3NND68ghCu1YIoePPQN1pGRABkJ6Bus96CutRZMydTl+TvuiRW1m3n0eDl0vRPcEysqdXn+jsQPsrHMquGeXEaY4Yk4wxWcY5V/9scqOMOVUFthatyTy8QyqwZ+kDURKoMWxNKr2EeqVKcTNOajqKoBgOE28U4tdQl5p5bwCw7BWquaZSzAPlwjlithJtp3pTImSqQRrb2Z8PHGigD4RZuNX6JYj6wj7O4TFLbCO/Mn/m8R+h6rYSUb3ekokRY6f/YukArN979jcW+V/S8g0eT/N3VN3kTqWbQ428m9/8k0P/1aIhF36PccEl6EhOcAUCrXKZXXWS3XKd2vc/TRBG9O5ELC17MmWubD2nKhUKZa26Ba2+D3P+4/MNCFwg59oWVeYhkzgN/JDR8deKBoD7Y+ljEjGZ0sosXVTvbc6RHirr2reNy1OXd6pJsQ+gqjk8VWFYmHrwBzW/n+uMPFiRwHB2I7ih8ciHFxIkd/3Omk5tCDV1t+2nNu5sxxpDFNx+huNhVT3/zMDz8usXC3ddaHBj1GHj/As08fwTS7Kt1HBTmyN29vdwAw+/wbwLVOJ3uAD1wi/dUH7Qei66PfyuRj4Ik9is+hglfbkbfR3cnZm7chlUWLdwmprtCohX4HUtlOcQjLYCu+fzGJH2QRKvP3UNz8bWk1qMxjGTOMThZ3kvgLI5AzFfo379UAAAAASUVORK5CYII="
            },
            {
                "enforcesSecureChat", false
            },
        };

        var statusResponsePacket = new StatusResponsePacket { Response = status };
        handler.PacketSerializer.Serialize(statusResponsePacket, writer);
    }

    public override string ToString()
    {
        return $"{ServerAddress}:{ServerPort} {Intent}, {ProtocolVersion}";
    }

}