using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(Notification notification)
        {
            await Clients.User(notification.ReceiverId.ToString()).SendAsync("ReceiveNotification", notification);
        }
        public async Task SendNotificationAllUsers(string message)
        {
            await Clients.All.SendAsync("ReceiveNotificationAllUsers", message);
        }
    }
}
