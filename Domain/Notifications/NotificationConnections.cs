using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public static class NotificationConnections
    {
        public static async Task SendNotificationToUserAsync(IHubContext<NotificationHub, INotificationHub> notificationHubContext, Notification notification, Guid? userId)
        {
            var userConnectionIds = NotificationHub.ConnectedUsers;
            if (userConnectionIds.TryGetValue(userId.ToString(), out var connectionIds))
            {
                foreach (var connectionId in connectionIds)
                {
                    await notificationHubContext.Clients.Client(connectionId).SendNotification(notification, connectionId);
                }
            }
            else
            {
                throw new Exception("User is not currently connected.");
            }
        }
    }

}
