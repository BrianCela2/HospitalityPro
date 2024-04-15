using DAL.Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    internal class NotificationRepository : BaseRepository<Notification, Guid>, INotificationRepository
    {
        public NotificationRepository(HospitalityProContext dbContext) : base(dbContext)
        {
        }

        public  IEnumerable<Notification> GetNotificationsUser(Guid receiverId)
        {
            var notifications =  context.Where(x => x.ReceiverId == receiverId).ToList();
            return notifications;
        }
        public IEnumerable<Notification> GetNotificationsUnSeen(string receiverId)
        {
            var notifications = context.Where(x => x.ReceiverId.ToString() == receiverId && x.IsSeen==false).ToList();
            return notifications;
        }
    }
}
