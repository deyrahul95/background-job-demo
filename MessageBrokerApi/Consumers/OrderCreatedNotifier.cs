using MassTransit;
using MessageBrokerApi.Events;

namespace MessageBrokerApi.Consumers;

public sealed class OrderCreatedNotifier(ILogger<OrderCreatedNotifier> logger) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        logger.LogInformation(
            "[Notification] Your order has been created. Order Id: {OrderId}, Created Time: {CreatedAt}, Message: {@Message}",
            message.OrderId,
            message.OrderCreatedTime,
            message);
        await Task.CompletedTask;
    }
}
