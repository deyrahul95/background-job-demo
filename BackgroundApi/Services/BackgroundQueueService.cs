using System.Threading.Channels;

namespace BackgroundApi.Services;

public class BackgroundQueueService<T>
{
    private readonly Channel<T> channel = Channel.CreateBounded<T>(new BoundedChannelOptions(100)
    {
        SingleReader = false,
        SingleWriter = true,
        FullMode = BoundedChannelFullMode.Wait,
    });

    public bool Enqueue(T item)
    {
        return channel.Writer.TryWrite(item);
    }

    public async ValueTask<T> DequeueAsync(CancellationToken token = default)
    {
        return await channel.Reader.ReadAsync(token);
    }

    public ValueTask<bool> WaitForNextRead(CancellationToken token = default)
    {
        return channel.Reader.WaitToReadAsync(token);
    }
}
