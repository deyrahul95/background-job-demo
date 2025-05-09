using System.Collections.Concurrent;
using BackgroundApi.Enums;
using BackgroundApi.Models;

namespace BackgroundApi.Services;

public class QueueReaderTwo(
    ILogger<QueueReaderTwo> logger,
    BackgroundQueueService<InventoryJob> queue,
    ConcurrentDictionary<int, JobStatus> statusDictionary) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await queue.WaitForNextRead(stoppingToken))
        {
            var item = await queue.DequeueAsync(stoppingToken);

            try
            {
                logger.LogInformation("Processing inventory check for order Id: {OrderId}", item.OrderId);
                statusDictionary[item.OrderId] = JobStatus.Processing;
                await ProcessAsync(item);
                logger.LogInformation("Inventory check completed for order Id: {OrderId}", item.OrderId);
            }
            catch (Exception ex)
            {
                statusDictionary[item.OrderId] = JobStatus.Failed;
                logger.LogError(ex, "Failed to process");
            }
        }
    }

    private Task ProcessAsync(InventoryJob job)
    {
        foreach (var item in job.ProductIds)
        {
            logger.LogInformation("Processing inventory check for product Id: {ProductId}", item);
            Task.Delay(200);
            logger.LogInformation("Inventory check completed for product Id: {ProductId}", item);
        }

        statusDictionary[job.OrderId] = JobStatus.Completed;
        return Task.CompletedTask;
    }
}