using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CreatorSuite
{
    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        public const string DefaultGroup = "Guest Users";

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
            //await Groups.AddToGroupAsync(Context.ConnectionId, DefaultGroup);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, DefaultGroup);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
