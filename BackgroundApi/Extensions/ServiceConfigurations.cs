using System.Collections.Concurrent;
using BackgroundApi.Enums;
using BackgroundApi.Models;
using BackgroundApi.Services;
using BackgroundApi.Services.Interfaces;

namespace BackgroundApi.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<BackgroundQueueService<InventoryJob>>();
        services.AddHostedService<QueueReaderOne>();
        // services.AddHostedService<QueueReaderTwo>();

        services.AddSingleton<ConcurrentDictionary<int, JobStatus>>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
