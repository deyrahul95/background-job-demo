namespace MessageBrokerApi.Constants;

public static class MassTransitConstants
{
    public const string RabbitMQHost = "rabbitmq://localhost"; 
    public const string RabbitMQUserName = "guest";
    public const string RabbitMQPassword = "guest";
    public const string OrderCreatedQueueName = "order-created-queue";
    public const string ClearCartQueueName = "clear-cart-queue";
}
