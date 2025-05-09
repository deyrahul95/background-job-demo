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
        services.AddHttpClient<IInventoryClient, InventoryClient>(option =>
        {
            option.BaseAddress = new Uri("http://localhost:5178");
            option.Timeout = TimeSpan.FromSeconds(1);
            option.DefaultRequestHeaders.Add("ContentType", "application/json");
        });

        services.AddSingleton<BackgroundQueueService<InventoryJob>>();
        services.AddHostedService<QueueReaderOne>();
        // services.AddHostedService<QueueReaderTwo>();

        services.AddSingleton<ConcurrentDictionary<int, JobStatusDto>>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
