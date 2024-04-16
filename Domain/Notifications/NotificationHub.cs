using Entities.Models;
using Microsoft.AspNetCore.SignalR;


namespace Domain.Notifications
{
    public class NotificationHub : Hub
    {

        public static Dictionary<string, List<string>> ConnectedUsers = new();
        public async Task SendNotification(Notification notification, string connectionId)
        {
            
            await Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
        }
        public string GetConnectionId(string userId) {
            lock (ConnectedUsers)
            {
                if (userId != null)
                {
                    if (!ConnectedUsers.ContainsKey(userId))
                        ConnectedUsers[userId] = new();
                    ConnectedUsers[userId].Add(Context.ConnectionId);

                }
            }
            return Context.ConnectionId;
        }
        public async Task SendNotificationAll(Notification notification)
        {
            await Clients.All.SendAsync("ReceiveNotificationAllUser",notification);
        }
        
    }
}
