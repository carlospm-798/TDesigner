using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;

string GetLocalIPAddress() {
    using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
    socket.Connect("8.8.8.8", 65530);
    var endPoint = socket.LocalEndPoint as IPEndPoint;
    return endPoint?.Address.ToString() ??  "127.0.0.1";
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});


var app = builder.Build();

string localIP = GetLocalIPAddress();

app.Urls.Add($"http://{localIP}:5171");
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();
app.MapHub<GameHub>("/gamehub");

app.Run();