using MassTransit;
using MessageBrokerApi.Constants;
using MessageBrokerApi.Consumers;
using MessageBrokerApi.Events;
using MessageBrokerApi.Messaging;

namespace MessageBrokerApi.Configs;

public static class MassTransitConfig
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<NotificationEmail<OrderCreatedEvent>>();
            x.AddConsumer<OrderCreatedNotifier>();
            x.AddConsumer<ClearCartNotifier>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(MassTransitConstants.RabbitMQHost, h =>
                {
                    h.Username(MassTransitConstants.RabbitMQUserName);
                    h.Password(MassTransitConstants.RabbitMQPassword);
                });

                cfg.ReceiveEndpoint(MassTransitConstants.OrderCreatedQueueName, e =>
                {
                    e.ConfigureConsumer<NotificationEmail<OrderCreatedEvent>>(context);
                    e.ConfigureConsumer<OrderCreatedNotifier>(context);
                    e.ConfigureConsumer<ClearCartNotifier>(context);
                });
            });
        });

        services.AddScoped<IEventBus, MassTransitEventBus>();

        return services;
    }
}
