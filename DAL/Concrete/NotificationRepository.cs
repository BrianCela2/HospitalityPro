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
    }
}
