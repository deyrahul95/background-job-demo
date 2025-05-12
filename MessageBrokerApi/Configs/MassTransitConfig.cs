using MassTransit;
using MessageBrokerApi.Constants;
using MessageBrokerApi.Consumers;
using MessageBrokerApi.Events;

namespace MessageBrokerApi.Configs;

public static class MassTransitConfig
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<NotificationEmail<OrderCreatedEvent>>();
            x.AddConsumer<OrderCreatedNotifier>();

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
                });
            });
        });

        return services;
    }
}
