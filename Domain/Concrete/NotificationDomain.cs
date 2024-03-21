using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using Domain.Notifications;
using DTO.RoomDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    internal class NotificationDomain:DomainBase,INotificationDomain
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public NotificationDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,IHubContext<NotificationHub> notificationHubContext) : base(unitOfWork, mapper, httpContextAccessor)
        {
            _notificationHubContext = notificationHubContext;
        }
        private INotificationRepository notificationRepository => _unitOfWork.GetRepository<INotificationRepository>();
        public async Task AddNotificationAsync(Notification notification)
        {
            await _notificationHubContext.Clients.User(notification.ReceiverId.ToString()).SendAsync("ReceiveNotification", notification);
            notificationRepository.Add(notification);
            _unitOfWork.Save();
        }
    }
}
