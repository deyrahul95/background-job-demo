using MassTransit;
using MessageBrokerApi.Events;

namespace MessageBrokerApi.Consumers;

public sealed class NotificationEmail<T>(ILogger<NotificationEmail<T>> logger) : IConsumer<T> where T : BaseEvent
{
    public async Task Consume(ConsumeContext<T> context)
    {
        var message = context.Message;
        logger.LogInformation(
            "[Notification] Email received. Type: {Type}, Time: {Time}, Message: {@Message}",
            message.Type,
            message.OccurredAt,
            message);
        await Task.CompletedTask;
    }
}
