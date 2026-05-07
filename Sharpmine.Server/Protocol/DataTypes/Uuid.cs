using System.Buffers.Binary;

namespace Sharpmine.Server.Protocol.DataTypes;

/// <summary>
/// Based on https://gist.github.com/MrZoidberg/9bac07cf3f5aa5896f75
///
/// Represents an immutable Java universally unique identifier (UUID).
/// A UUID represents a 128-bit value.
/// </summary>
public readonly struct Uuid : IEquatable<Uuid>
{

    public static readonly Uuid Empty = new();

    /// <summary>
    /// Constructs a new UUID using the specified data.
    /// </summary>
    /// <param name="mostSignificantBits">The most significant 64 bits of the UUID.</param>
    /// <param name="leastSignificantBits">The least significant 64 bits of the UUID</param>
    public Uuid(long mostSignificantBits, long leastSignificantBits)
    {
        MostSignificantBits = mostSignificantBits;
        LeastSignificantBits = leastSignificantBits;
    }

    /// <summary>
    /// Constructs a new UUID using the specified data.
    /// </summary>
    /// <param name="bytes">Byte array that represents the UUID.</param>
    public Uuid(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != 16)
        {
            throw new ArgumentException("Length of the UUID byte array must be 16");
        }

        MostSignificantBits = BinaryPrimitives.ReadInt64BigEndian(bytes[..8]);
        LeastSignificantBits = BinaryPrimitives.ReadInt64BigEndian(bytes[8..]);
    }

    /// <summary>
    /// The most significant 64 bits of this UUID's 128 bit value.
    /// </summary>
    public long MostSignificantBits { get; }

    /// <summary>
    /// The least significant 64 bits of this UUID's 128 bit value.
    /// </summary>
    public long LeastSignificantBits { get; }

    /// <summary>
    /// Creates a UUID from the string standard representation as described in the <see cref="ToString()"/> method.
    /// </summary>
    /// <param name="input">A string that specifies a UUID.</param>
    /// <returns>A UUID with the specified value.</returns>
    /// <exception cref="ArgumentNullException">input is null.</exception>
    /// <exception cref="FormatException">input is not in a recognized format.</exception>
    public static Uuid Parse(string input) => Guid.Parse(input);

    public static Uuid NewUuid() => Guid.NewGuid();

    /// <summary>Converts a <see cref="T:System.Guid" /> to an <see cref="T:Uuid"/>.</summary>
    public static implicit operator Uuid(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Empty;
        }

        Span<byte> uuidBytes = stackalloc byte[16];
        Span<byte> guidBytes = stackalloc byte[16];
        value.TryWriteBytes(guidBytes);

        // MSB Mapping
        uuidBytes[0] = guidBytes[6];
        uuidBytes[1] = guidBytes[7];
        uuidBytes[2] = guidBytes[4];
        uuidBytes[3] = guidBytes[5];
        uuidBytes[4] = guidBytes[0];
        uuidBytes[5] = guidBytes[1];
        uuidBytes[6] = guidBytes[2];
        uuidBytes[7] = guidBytes[3];

        // LSB Mapping
        uuidBytes[8] = guidBytes[15];
        uuidBytes[9] = guidBytes[14];
        uuidBytes[10] = guidBytes[13];
        uuidBytes[11] = guidBytes[12];
        uuidBytes[12] = guidBytes[11];
        uuidBytes[13] = guidBytes[10];
        uuidBytes[14] = guidBytes[9];
        uuidBytes[15] = guidBytes[8];

        return new Uuid(
            BinaryPrimitives.ReadInt64BigEndian(uuidBytes[..8]),
            BinaryPrimitives.ReadInt64BigEndian(uuidBytes[8..]));
    }

    /// <summary>Converts an <see cref="T:Uuid"/> to a <see cref="T:System.Guid" />.</summary>
    public static implicit operator Guid(Uuid uuid)
    {
        if (uuid == Empty)
        {
            return Guid.Empty;
        }

        Span<byte> guidBytes = stackalloc byte[16];
        Span<byte> uuidBytes = stackalloc byte[16];
        BinaryPrimitives.WriteInt64BigEndian(uuidBytes[..8], uuid.MostSignificantBits);
        BinaryPrimitives.WriteInt64BigEndian(uuidBytes[8..], uuid.LeastSignificantBits);

        // MSB Mapping
        guidBytes[4] = uuidBytes[2];
        guidBytes[5] = uuidBytes[3];
        guidBytes[6] = uuidBytes[0];
        guidBytes[7] = uuidBytes[1];
        guidBytes[0] = uuidBytes[4];
        guidBytes[1] = uuidBytes[5];
        guidBytes[2] = uuidBytes[6];
        guidBytes[3] = uuidBytes[7];

        // LSB Mapping
        guidBytes[15] = uuidBytes[8];
        guidBytes[14] = uuidBytes[9];
        guidBytes[13] = uuidBytes[10];
        guidBytes[12] = uuidBytes[11];
        guidBytes[11] = uuidBytes[12];
        guidBytes[10] = uuidBytes[13];
        guidBytes[9] = uuidBytes[14];
        guidBytes[8] = uuidBytes[15];

        return new Guid(guidBytes);
    }

    /// <summary>
    ///  Returns a 16-element byte array that contains the value of this instance.
    /// </summary>
    /// <returns>A 16-element byte array</returns>
    public byte[] ToByteArray()
    {
        byte[] bytes = new byte[16];
        TryWriteBytes(bytes);
        return bytes;
    }

    /// <summary>
    /// Writes the UUID bytes to a destination span (Zero Allocation).
    /// </summary>
    public bool TryWriteBytes(Span<byte> destination)
    {
        if (destination.Length < 16)
        {
            return false;
        }

        BinaryPrimitives.WriteInt64BigEndian(destination[..8], MostSignificantBits);
        BinaryPrimitives.WriteInt64BigEndian(destination[8..], LeastSignificantBits);
        return true;
    }

    /// <summary>Indicates whether the values of two specified <see cref="T:Uuid" /> objects are equal.</summary>
    /// <returns>true if <paramref name="a" /> and <paramref name="b" /> are equal; otherwise, false.</returns>
    /// <param name="a">The first object to compare. </param>
    /// <param name="b">The second object to compare. </param>
    public static bool operator ==(Uuid a, Uuid b) => a.Equals(b);

    /// <summary>Indicates whether the values of two specified <see cref="T:Uuid" /> objects are not equal.</summary>
    /// <returns>true if <paramref name="a" /> and <paramref name="b" /> are not equal; otherwise, false.</returns>
    /// <param name="a">The first object to compare. </param>
    /// <param name="b">The second object to compare. </param>
    public static bool operator !=(Uuid a, Uuid b) => !a.Equals(b);

    /// <summary>
    /// Returns a value that indicates whether this instance is equal to a specified
    /// object.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>true if o is a <paramref name="obj"/> that has the same value as this instance; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is Uuid uuid && Equals(uuid);

    /// <summary>
    /// Returns a value that indicates whether this instance and a specified <see cref="Uuid"/>
    /// object represent the same value.
    /// </summary>
    /// <param name="uuid">An object to compare to this instance.</param>
    /// <returns>true if <paramref name="uuid"/> is equal to this instance; otherwise, false.</returns>
    public bool Equals(Uuid uuid)
    {
        return MostSignificantBits == uuid.MostSignificantBits
               && LeastSignificantBits == uuid.LeastSignificantBits;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode() => ((Guid) this).GetHashCode();

    /// <summary>
    /// Returns a String object representing this UUID.
    /// </summary>
    /// <returns>A string representation of this UUID.</returns>
    public override string ToString() => ((Guid) this).ToString();

}
