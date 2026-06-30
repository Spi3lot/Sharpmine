using Raspite;
using Raspite.Tags;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes;

public static class NbtExtensions
{

    public static async Task<bool> TryLoadFromNbtFileAsync<TTag>(this ILoadableFromNbt<TTag> loadable, string path)
        where TTag : class, ITag
    {
        try
        {
            if (File.Exists(path) && TagSerializer.TryParse(await File.ReadAllBytesAsync(path), out TTag? tag))
            {
                loadable.LoadFromNbt(tag);
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public static Task WriteToNbtFileAsync<TTag>(this IConvertibleToNbt<TTag> convertible,
        string path,
        string rootName = "",
        int initialBufferSize = PooledByteBufferWriter.DefaultInitialBufferSize,
        bool backupExisting = false) where TTag : class, ITag
    {
        try
        {
            if (backupExisting && File.Exists(path))
            {
                File.Move(path, $"{path}_old", overwrite: true);
            }

            using var pooledWriter = new PooledByteBufferWriter(initialBufferSize);
            pooledWriter.WriteNbt(convertible.ToNbt(rootName), network: false);
            return File.WriteAllBytesAsync(path, pooledWriter.WrittenMemory);
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
    }

}
