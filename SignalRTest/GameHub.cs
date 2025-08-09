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
    private static string? HostConnectionId = null;
    public override Task OnConnectedAsync() {
        ConnectedClients.TryAdd(Context.ConnectionId, Context.ConnectionId);

        if (HostConnectionId == null) {
            HostConnectionId = Context.ConnectionId;
        }
        
        return Clients.All.SendAsync("UpdateClientList",  ConnectedClients.Keys.ToList())
            .ContinueWith(_ => Clients.Client(Context.ConnectionId).SendAsync("IdentifyHost", Context.ConnectionId == HostConnectionId))
            .ContinueWith(_ => base.OnConnectedAsync());
    }

    public override Task OnDisconnectedAsync(Exception? exception) {
        ConnectedClients.TryRemove(Context.ConnectionId, out _);

        if (Context.ConnectionId == HostConnectionId) {
            HostConnectionId = ConnectedClients.Keys.FirstOrDefault();
        }
        
        return Clients.All.SendAsync("UpdateClientList", ConnectedClients.Keys.ToList())
            .ContinueWith(_ => base.OnDisconnectedAsync(exception));
    }

    private Task UpdateClientList() {
        var clientList = ConnectedClients.Keys.ToList();
        return Clients.All.SendAsync("UpdateClientList", clientList);
    }

    public async Task CreateLobby(){
        if (Context.ConnectionId != HostConnectionId){
            await Clients.Caller.SendAsync("Error",
                "No tienes permisos para crear un lobby");
            return;
        }
        
        await Clients.All.SendAsync("LobbyCreated",
            $"{Context.ConnectionId} ha creado el lobby");
    }
    
}