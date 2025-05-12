using MessageBrokerApi.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessaging();

var app = builder.Build();

app.MapEndpoints();

app.Run();