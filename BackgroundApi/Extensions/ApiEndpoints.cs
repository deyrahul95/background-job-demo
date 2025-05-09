using BackgroundApi.Services;
using BackgroundApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundApi.Extensions;

public static class ApiEndpoints
{
    private const string HealthTag = "Health";
    private const string QueueTag = "Queue";
    private const string OrderTag = "Order";
    private const string HealthRoute = "/api/health";
    private const string AddToQueueRoute = "/api/addToQueue/{count:int}";
    private const string CreateOrderRoute = "/api/orders";
    private const string GetOrderRoute = "/api/orders/{id:int}";

    public static WebApplication AddApiEndpoints(this WebApplication app)
    {
        // /api/health
        app.MapGet(
            HealthRoute,
            () => Results.Ok(new { Message = "Api is healthy" }))
            .WithTags(HealthTag);

        // /api/addToQueue
        app.MapPost(
            AddToQueueRoute,
            async ([FromServices] BackgroundQueueService<string> queue, int count) =>
            {
                for (int i = 0; i < count; i++)
                {
                    queue.Enqueue($"Pushed {i} msg to queue");
                    await Task.Delay(10);
                }

                return Results.Accepted();
            })
            .WithTags(QueueTag);

        // /api/orders
        app.MapPost(
            CreateOrderRoute,
            async ([FromServices] IOrderService orderService, [FromBody] int itemCount) =>
            {
                var status = await orderService.CreateOrder(itemCount);
                await Task.Delay(10);
                return Results.Accepted();
            })
            .WithTags(OrderTag);

        app.MapGet(
            GetOrderRoute,
            async ([FromServices] IOrderService orderService, int id) =>
            {
                var status = await orderService.GetOrder(id);
                await Task.Delay(10);
                return Results.Ok(new { Id = id, Status = status.ToString() });
            })
            .WithTags(OrderTag);

        return app;
    }
}
