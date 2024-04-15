using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public class NotificationHub : Hub
    {
        public static string connectionID ;
        public async Task SendNotification(Notification notification, string connectionId) =>
         await Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
        public string GetConnectionId() {
            connectionID = Context.ConnectionId;
            return Context.ConnectionId;
        }
        public async Task SendNotificationAll(string message)
        {
            await Clients.All.SendAsync("ReceiveNotificationAllUser",message);
        }
    }
}
