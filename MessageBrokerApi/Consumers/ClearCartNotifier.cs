using MassTransit;
using MessageBrokerApi.Events;

namespace MessageBrokerApi.Consumers;

public class ClearCartNotifier(ILogger<ClearCartNotifier> logger) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        logger.LogInformation("Cart cleared. User Id: {UserId}", message.UserId);
        await Task.CompletedTask;
    }
}
