
using MassTransit;
using MessageBrokerApi.Events;

namespace MessageBrokerApi.Messaging;

public sealed class MassTransitEventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    public Task PublishAsync<T>(T @event) where T : BaseEvent
    {
        return publishEndpoint.Publish(@event);
    }
}
