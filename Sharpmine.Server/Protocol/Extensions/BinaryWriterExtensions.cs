namespace Sharpmine.Server.Protocol.Extensions;

public static class BinaryWriterExtensions
{

    extension(BinaryWriter writer)
    {

        public void WritePrefixedOptional(string? value)
        {
            bool isPresent = value is not null;
            writer.Write(isPresent);

            if (isPresent)
            {
                writer.Write(value!);
            }
        }

    }

}
