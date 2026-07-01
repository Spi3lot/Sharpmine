using Raspite;
using Raspite.Tags;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Domain.DataTypes.Nbt;

public static class NbtExtensions
{

    extension<TTag>(ILoadableFromNbt<TTag> loadable) where TTag : class, ITag
    {

        public async Task<bool> LoadFromNbtFileAsync(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            if (!TagSerializer.TryParse(await File.ReadAllBytesAsync(path), out TTag? tag))
            {
                throw new InvalidDataException($"Failed to parse NBT data from file: {path}");
            }

            loadable.LoadFromNbt(tag);
            return true;
        }

    }

    extension<TTag>(IConvertibleToNbt<TTag> convertible) where TTag : class, ITag
    {

        public Task WriteToNbtFileAsync(
            string path,
            string rootName = "",
            int initialBufferSize = PooledByteBufferWriter.DefaultInitialBufferSize,
            bool backupExisting = false)
        {
            return convertible.WriteToNbtFileAsync(
                path,
                rootName,
                initialBufferSize,
                (backupExisting) ? path + "_old" : null);
        }

        public async Task WriteToNbtFileAsync(
            string path,
            string rootName = "",
            int initialBufferSize = PooledByteBufferWriter.DefaultInitialBufferSize,
            string? backupPath = null)
        {
            string tempPath = $"{path}.{Guid.CreateVersion7():N}.tmp";

            try
            {
                using (var pooledWriter = new PooledByteBufferWriter(initialBufferSize))
                {
                    pooledWriter.WriteNbt(convertible.ToNbt(rootName), network: false);
                    await File.WriteAllBytesAsync(tempPath, pooledWriter.WrittenMemory);
                }

                if (File.Exists(path))
                {
                    File.Replace(
                        sourceFileName: tempPath,
                        destinationFileName: path,
                        destinationBackupFileName: backupPath,
                        ignoreMetadataErrors: true);
                }
                else
                {
                    File.Move(tempPath, path);
                }
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
        }

    }

}
