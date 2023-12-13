using Chat.Domain.DTO;
using Chat.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Domain.Hubs
{
    internal sealed class ChatHub : Hub
    {
        private readonly IChatServices __chatServices;
        public ChatHub(IChatServices chatServices)
        {
            __chatServices = chatServices;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "ChatRoom");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChatRoom");

            var user = __chatServices.GetUserByConnectionId(Context.ConnectionId);
            await __chatServices.RemoveUser(user);
            await GetOnlineUsers();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string name)
        {
            await __chatServices.AddUserConnectionId(Context.ConnectionId, name);

            await GetOnlineUsers();
        }

        private async Task GetOnlineUsers()
        {
            var onlineUsers = __chatServices.GetOnlineUsers();
            await Clients.Groups("ChatRoom").SendAsync("OnlineUsers", onlineUsers);
        }

        public async Task ReceiveMessage(MessageDTO message)
        {
            await Clients.Group("ChatRoom").SendAsync("NewMessage", message);
        }

        public async Task CreatePrivateChat(MessageDTO message)
        {
            string privateGroupName = GetPrivateGrouName(from: message.From, to: message.To);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = __chatServices.GetConnectionIdByUser(user: message.To);

            await Groups.AddToGroupAsync(toConnectionId, privateGroupName);

            await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
        }

        public async Task ReceivePrivateMessage(MessageDTO message)
        {
            string privateGroupName = GetPrivateGrouName(from: message.From, to: message.To);
            await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
        }

        public async Task RemovePrivateChat(string from, string to)
        {
            string privateGroupName = GetPrivateGrouName(from: from, to: to);
            await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = __chatServices.GetConnectionIdByUser(user: to);

            await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
        }

        private string GetPrivateGrouName(string from, string to)
        {
            var stringCompare = string.CompareOrdinal(strA: from, strB: to) < 0;
            return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
        }
    }
}
