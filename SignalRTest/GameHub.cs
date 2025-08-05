#nullable enable

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

public class GameHub : Hub {
    private static ConcurrentDictionary<string, string> ConnectedClients = new();

    public override Task OnConnectedAsync() {
        ConnectedClients.TryAdd(Context.ConnectionId, Context.ConnectionId);
        return Clients.All.SendAsync("UpdateClientList",  ConnectedClients.Keys.ToList())
            .ContinueWith(_ => base.OnConnectedAsync());
    }

    public override Task OnDisconnectedAsync(Exception? exception) {
        ConnectedClients.TryRemove(Context.ConnectionId, out _);
        return Clients.All.SendAsync("UpdateClientList", ConnectedClients.Keys.ToList())
            .ContinueWith(_ => base.OnDisconnectedAsync(exception));
    }

    private Task UpdateClientList() {
        var clientList = ConnectedClients.Keys.ToList();
        return Clients.All.SendAsync("UpdateClientList", clientList);
    }
}