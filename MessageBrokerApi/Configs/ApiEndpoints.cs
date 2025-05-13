using MessageBrokerApi.Events;
using MessageBrokerApi.Messaging;
using MessageBrokerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageBrokerApi.Configs;

public static class ApiEndpoints
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // POST api/orders/
        app.MapPost("/api/orders", async (
            [FromServices] IEventBus eventBus,
            [FromBody] CreateOrderRequest request) =>
        {
            int orderId = 100;
            DateTime createdAt = DateTime.UtcNow;

            var orderCreatedEvent = new OrderCreatedEvent(
                OrderId: orderId,
                UserId: 2,
                OrderCreatedTime: createdAt);
            await eventBus.PublishAsync(orderCreatedEvent);

            return Results.Created(new Uri($"/api/orders/{orderId}"), orderId);
        });

        return app;
    }
}
