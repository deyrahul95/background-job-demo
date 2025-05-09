
namespace BackgroundApi.Services;

public class QueueReaderTwo(BackgroundQueueService<string> queue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await queue.WaitForNextRead(stoppingToken))
        {
            var item = await queue.DequeueAsync(stoppingToken);
            Console.WriteLine($"{nameof(QueueReaderTwo)} -> Value: {item}");
        }
    }
}