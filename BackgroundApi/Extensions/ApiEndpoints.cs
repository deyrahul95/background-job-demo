using BackgroundApi.Models;
using BackgroundApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundApi.Extensions;

public static class ApiEndpoints
{
    private const string HealthTag = "Health";
    private const string OrderTag = "Order";
    private const string InventoryTag = "Inventory";
    private const string HealthRoute = "/api/health";
    private const string CreateOrderRoute = "/api/orders";
    private const string GetOrderStatusRoute = "/api/orders/{id:int}/status";
    private const string GetInventory = "/api/inventory/{id:int}";

    public static WebApplication AddApiEndpoints(this WebApplication app)
    {
        // /api/health
        app.MapGet(
            HealthRoute,
            () => Results.Ok(new { Message = "Api is healthy" }))
            .WithTags(HealthTag);

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
            GetOrderStatusRoute,
            async ([FromServices] IOrderService orderService, int id) =>
            {
                var status = await orderService.GetOrder(id);
                await Task.Delay(10);

                if (status is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(new { Id = id, Status = status?.Status.ToString(), status?.Message });
            })
            .WithTags(OrderTag);

        // /api/inventory
        app.MapGet(
            GetInventory,
            async (HttpRequest request, int id) =>
            {
                if (request.Headers.TryGetValue("Authorization", out var authHeader) is false)
                {
                    return Results.Unauthorized();
                }

                await Task.Delay(TimeSpan.FromMilliseconds(300));
                var random = Random.Shared.Next(1, 100);
                return TypedResults.Ok(new GetInventoryResponse(ProductId: id, AvailableQuantity: random));
            })
            .WithTags(InventoryTag);

        return app;
    }
}
