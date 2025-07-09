using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ShipConnect.Hubs
{
    [Authorize]
    public class NotificationHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            Console.WriteLine($"User connected: {userId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            Console.WriteLine($"User disconnected: {userId}");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
