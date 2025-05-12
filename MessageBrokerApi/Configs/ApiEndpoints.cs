using MassTransit;
using MessageBrokerApi.Events;
using MessageBrokerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageBrokerApi.Configs;

public static class ApiEndpoints
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // POST api/orders/
        app.MapPost("/api/orders", async (
            [FromServices] IPublishEndpoint publishEndpoint,
            [FromBody] CreateOrderRequest request) => {
            int orderId = 100;
            DateTime createdAt = DateTime.UtcNow;

            var orderCreatedEvent = new OrderCreatedEvent(
                OrderId: orderId,
                UserId: 2,
                OrderCreatedTime: createdAt);
            await publishEndpoint.Publish(orderCreatedEvent);

            return Results.Created(new Uri($"/api/orders/{orderId}"), orderId);
        });

        return app;
    }
}
