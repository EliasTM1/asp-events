using Microsoft.AspNetCore.SignalR;

namespace EventApi.Hubs;

public class EventHub : Hub
{
    private static Timer? _timer;
    private static IHubContext<EventHub>? _hubContext;
    private static int _eventCounter = 0;

    public static void StartEventBroadcasting(IHubContext<EventHub> hubContext)
    {
        _hubContext = hubContext;

        // Broadcast events every 3 seconds
        _timer = new Timer(async _ =>
        {
            _eventCounter++;
            var eventData = new
            {
                Id = _eventCounter,
                Message = $"Server Event #{_eventCounter}",
                Timestamp = DateTime.UtcNow
            };

            await _hubContext.Clients.All.SendAsync("ReceiveEvent", eventData);
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceiveEvent", new
        {
            Id = 0,
            Message = "Connected to EventHub!",
            Timestamp = DateTime.UtcNow
        });

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
