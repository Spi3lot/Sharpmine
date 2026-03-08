namespace Sharpmine.Server.Packets;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class PacketAttribute(int id, ConnectionState connectionState) : Attribute;