using MessageBrokerApi.Events;

namespace MessageBrokerApi.Messaging;

public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : BaseEvent;
}
