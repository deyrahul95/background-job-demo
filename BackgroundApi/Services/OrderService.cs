using System.Collections.Concurrent;
using BackgroundApi.Enums;
using BackgroundApi.Models;
using BackgroundApi.Services.Interfaces;

namespace BackgroundApi.Services;

public class OrderService(
    ConcurrentDictionary<int, JobStatus> statusDictionary,
    BackgroundQueueService<InventoryJob> queue) : IOrderService
{
    public Task<JobStatus> CreateOrder(int itemCount)
    {
        List<int> productIds = [];

        for (int i = 0; i < itemCount; i++)
        {
            productIds.Add(i);
        }

        queue.Enqueue(new InventoryJob(OrderId: 100, ProductIds: productIds, Quantity: 5));

        return Task.FromResult(JobStatus.Queue);
    }

    public Task<JobStatus> GetOrder(int id)
    {
        statusDictionary.TryGetValue(id, out var status);
        return Task.FromResult(status);
    }
}
