using BackgroundApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundApi.Extensions;

public static class ApiEndpoints
{
    private const string HealthTag = "Health";
    private const string QueueTag = "Queue";
    private const string HealthRoute = "/api/health";
    private const string AddToQueueRoute = "/api/addToQueue/{count:int}";

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

        return app;
    }
}
