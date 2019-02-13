using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CreatorSuite
{
    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        public ChatHub()
        {
        }

        public async Task SendMessageTo(string user, string message)
        {
            await Clients.All.ReceiveMessageFrom(user, message);
        }

        public async Task SendMessageToAll(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }

        public void ReceiveMessage(string message)
        {
        }

        public override async Task OnConnectedAsync()
        {
            await SendMessageToAll($"Well {Context.ConnectionId} connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await SendMessageToAll($"Well {Context.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
