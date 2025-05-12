namespace MessageBrokerApi.Events;

public sealed record OrderCreatedEvent(
    int OrderId,
    int UserId,
    DateTime OrderCreatedTime) : BaseEvent(Type: nameof(OrderCreatedEvent), OccurredAt: DateTime.UtcNow);
