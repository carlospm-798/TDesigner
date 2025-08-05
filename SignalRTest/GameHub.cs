using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class GameHub : Hub {
    private static ConcurrentDictionary<string, string> ConnectedClients = new();

    public override Task OnConnectedAsync() {
        ConnectedClients.TryAdd(Context.ConnectionId, Context.ConnectionId);
        ConnectedClients.All.SendAsync("UpdateClientList", ConnectedClients.Keys.ToList());
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception) {
        ConnectedClients.TryRemove(Context.ConnectionId, out _);
        Clients.All.SendAsync("UpdateClientList", ConnectedClients.Keys.ToList());
        return base.OnDisconnectedAsync(exception);
    }
    
}