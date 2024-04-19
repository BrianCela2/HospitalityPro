using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public interface INotificationHub
    {
        Task SendNotification(Notification notification, string connectionId);
        Task SendNotificationAll(Notification notification);
    }
}
