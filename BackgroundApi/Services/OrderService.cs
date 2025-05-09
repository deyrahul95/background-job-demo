using System.Collections.Concurrent;
using BackgroundApi.Enums;
using BackgroundApi.Models;
using BackgroundApi.Services.Interfaces;
using Microsoft.Extensions.Primitives;

namespace BackgroundApi.Services;

public class OrderService(
    IHttpContextAccessor httpContextAccessor,
    ConcurrentDictionary<int, JobStatusDto> statusDictionary,
    BackgroundQueueService<InventoryJob> queue,
    ILogger<OrderService> logger) : IOrderService
{
    public Task<JobStatus> CreateOrder(int itemCount)
    {
        List<int> productIds = [];

        for (int i = 1; i <= itemCount; i++)
        {
            productIds.Add(i);
        }

        var context = httpContextAccessor.HttpContext;
        StringValues token = string.Empty;

        context?.Request.Headers.TryGetValue("Authorization", out token);

        queue.Enqueue(new InventoryJob(
            OrderId: 100,
            ProductIds: productIds,
            Quantity: 5,
            Token: token));

        return Task.FromResult(JobStatus.Queue);
    }

    public Task<JobStatusDto?> GetOrder(int id)
    {
        statusDictionary.TryGetValue(id, out var status);
        logger.LogInformation(
            "[OrderService] Order Id: {OrderId}, Status: {Status}",
            id,
            status?.Status.ToString() ?? "N/A");
        return Task.FromResult(status);
    }
}
