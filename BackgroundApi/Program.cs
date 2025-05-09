using BackgroundApi.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BackgroundQueueService<string>>();
builder.Services.AddHostedService<QueueReaderOne>();
builder.Services.AddHostedService<QueueReaderTwo>();

var app = builder.Build();

app.MapGet(
    "/api/health",
    () => Results.Ok(new { Message = "Api is healthy" }));

app.MapPost(
    "/api/addToQueue/{count:int}", 
    async ([FromServices] BackgroundQueueService<string> queue, int count ) =>
    {
        for (int i = 0; i < count; i++)
        {
            queue.Enqueue($"Pushed {i} msg to queue");
            await Task.Delay(10);
        }

        return Results.Accepted();
    });

app.Run();