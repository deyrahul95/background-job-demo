using System.Collections.Concurrent;
using BackgroundApi.Enums;
using BackgroundApi.Models;
using BackgroundApi.Services.Interfaces;

namespace BackgroundApi.Services;

public class QueueReaderOne(
    ILogger<QueueReaderOne> logger,
    BackgroundQueueService<InventoryJob> queue,
    IInventoryClient inventoryClient,
    ConcurrentDictionary<int, JobStatusDto> statusDictionary) : BackgroundService
{
    private readonly string QueueName = "[QueueReaderOne]";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await queue.WaitForNextRead(stoppingToken))
        {
            var item = await queue.DequeueAsync(stoppingToken);

            try
            {
                logger.LogInformation(
                    "{QueueName} Processing inventory check for order Id: {OrderId}",
                    QueueName,
                    item.OrderId);
                statusDictionary[item.OrderId] = new JobStatusDto(JobStatus.Processing);
                await ProcessAsync(item);
                logger.LogInformation(
                    "{QueueName} Inventory check completed for order Id: {OrderId}",
                    QueueName,
                    item.OrderId);
            }
            catch (Exception ex)
            {
                statusDictionary[item.OrderId] = new JobStatusDto(Status: JobStatus.Failed, Message: ex.Message);
                logger.LogError(exception: ex, message: "Failed to process");
            }
        }
    }

    private async Task ProcessAsync(InventoryJob job)
    {
        foreach (var item in job.ProductIds)
        {
            logger.LogInformation(
                "{QueueName} Processing inventory check for product Id: {ProductId}",
                QueueName,
                item);
            await Task.Delay(TimeSpan.FromMilliseconds(100));

            var response = await inventoryClient.GetInventory(item, job.Token);

            if (response is null || response.AvailableQuantity < job.Quantity)
            {
                logger.LogWarning("Product is out of stock! {@Response}", response);
                statusDictionary[job.OrderId] = new JobStatusDto(
                    Status: JobStatus.Failed,
                    Message: $"Product is out of stock. Requested quantity: {job.Quantity}, Available quality: {response?.AvailableQuantity}");
                return;
            }

            logger.LogInformation(
                "{QueueName} Inventory check completed for product Id: {ProductId}",
                QueueName,
                item);
        }

        statusDictionary[job.OrderId] = new JobStatusDto(Status: JobStatus.Completed, Message: "Inventory check completed");
        await Task.CompletedTask;
        return;
    }
}