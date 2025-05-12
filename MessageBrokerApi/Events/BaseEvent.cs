namespace MessageBrokerApi.Events;

public record BaseEvent(string Type, DateTime OccurredAt);
