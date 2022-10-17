
using Microsoft.AspNetCore.SignalR;
namespace education_project_backend.SignalR.Chat
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("ChatHub hub connected");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("ChatHub hub disconnected");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine("Message received");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
