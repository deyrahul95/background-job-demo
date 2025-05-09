using BackgroundApi.Extensions;
using BackgroundApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BackgroundQueueService<string>>();
builder.Services.AddHostedService<QueueReaderOne>();
builder.Services.AddHostedService<QueueReaderTwo>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddApiEndpoints();

app.Run();