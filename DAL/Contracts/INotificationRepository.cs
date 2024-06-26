﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface INotificationRepository : IRepository<Notification, Guid>
    {
        IEnumerable<Notification> GetNotificationsUser(Guid receiverId);
        IEnumerable<Notification> GetNotificationsUnSeen(string receiverId);
    }

}
