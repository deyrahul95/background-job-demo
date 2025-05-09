using BackgroundApi.Extensions;
using BackgroundApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BackgroundQueueService<string>>();
builder.Services.AddHostedService<QueueReaderOne>();
builder.Services.AddHostedService<QueueReaderTwo>();

var app = builder.Build();

app.AddApiEndpoints();

app.Run();