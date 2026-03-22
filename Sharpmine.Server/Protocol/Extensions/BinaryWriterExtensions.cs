namespace Sharpmine.Server.Protocol.Extensions;

public static class BinaryWriterExtensions
{

    extension(BinaryWriter writer)
    {

        public void WritePrefixedOptional<T>(T? value, Action<T> writeAction)
        {
            bool isPresent = value is not null;
            writer.Write(isPresent);

            if (isPresent)
            {
                writeAction(value!);
            }
        }

        public void WritePrefixedArray<T>(ReadOnlySpan<T> value, Action<T> writeElementAction)
        {
            writer.Write7BitEncodedInt(value.Length);

            foreach (T element in value)
            {
                writeElementAction(element);
            }
        }

    }

}
