using EventApi.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

// Configure CORS for Angular app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

// Map SignalR hub
app.MapHub<EventHub>("/eventhub");

// Start broadcasting events
EventHub.StartEventBroadcasting(app.Services.GetRequiredService<IHubContext<EventHub>>());

app.Run();
