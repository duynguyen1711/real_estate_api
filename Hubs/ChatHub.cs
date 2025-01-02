using Microsoft.AspNetCore.SignalR;
using real_estate_api.Models;
namespace real_estate_api.Hubs;
public class ChatHub : Hub 
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessageÁsync(string user,string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
