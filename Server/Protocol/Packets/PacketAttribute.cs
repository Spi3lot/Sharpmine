namespace Sharpmine.Server.Protocol.Packets;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class PacketAttribute(int id, ConnectionState connectionState) : Attribute;