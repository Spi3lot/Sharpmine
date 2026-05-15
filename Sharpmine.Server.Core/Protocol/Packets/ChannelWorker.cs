using System.Threading.Channels;

namespace Sharpmine.Server.Core.Protocol.Packets;

public abstract class ChannelWorker<T>(Channel<T> channel)
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        T? currentItem = default;

        try
        {
            await foreach (T item in channel.Reader.ReadAllAsync(cancellationToken))
            {
                currentItem = item;
                await ProcessAsync(currentItem, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Shutting down
        }
        catch (Exception ex)
        {
            await OnErrorAsync(ex, currentItem);
        }
    }

    protected abstract ValueTask ProcessAsync(T currentItem, CancellationToken cancellationToken);

    protected abstract Task OnErrorAsync(Exception ex, T? currentItem);

}
