using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);

            await SendMessage(string.Empty, $"{ username} entrou");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await SendMessage(string.Empty, $"{username} saiu");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
