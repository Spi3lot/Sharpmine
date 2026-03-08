namespace Sharpmine.Server.Packets;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class PacketAttribute(int id) : Attribute
{

    public int Id { get; } = id;

}