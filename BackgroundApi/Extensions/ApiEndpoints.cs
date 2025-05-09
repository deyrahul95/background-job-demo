using BackgroundApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundApi.Extensions;

public static class ApiEndpoints
{
    private const string HealthTag = "Health";
    private const string QueueTag = "Queue";
    private const string HealthRoute = "Health Check";
    private const string AddToQueueRoute = "Add To Queue";

    public static WebApplication AddApiEndpoints(this WebApplication app)
    {
        // /api/health
        app.MapGet(
            "/api/health",
            () => Results.Ok(new { Message = "Api is healthy" }))
            .WithTags(HealthTag)
            .WithName(HealthRoute);

        // /api/addToQueue
        app.MapPost(
            "/api/addToQueue/{count:int}",
            async ([FromServices] BackgroundQueueService<string> queue, int count) =>
            {
                for (int i = 0; i < count; i++)
                {
                    queue.Enqueue($"Pushed {i} msg to queue");
                    await Task.Delay(10);
                }

                return Results.Accepted();
            })
            .WithTags(QueueTag)
            .WithName(AddToQueueRoute);

        return app;
    }
}
