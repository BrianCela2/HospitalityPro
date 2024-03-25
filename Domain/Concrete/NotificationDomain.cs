using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using Domain.Notifications;
using DTO.NotificationDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
namespace Domain.Concrete
{
    internal class NotificationDomain : DomainBase, INotificationDomain
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public NotificationDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> notificationHubContext) : base(unitOfWork, mapper, httpContextAccessor)
        {
            _notificationHubContext = notificationHubContext;
        }
        private INotificationRepository notificationRepository => _unitOfWork.GetRepository<INotificationRepository>();
        private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();
        public async Task AddNotificationAsync(CreateNotificationDTO Createnotification)
        {
            await _notificationHubContext.Clients.User(Createnotification.ReceiverId.ToString()).SendAsync("ReceiveNotification", Createnotification);
            var notification = _mapper.Map<Notification>(Createnotification);
            notificationRepository.Add(notification);
            _unitOfWork.Save();
        }
        public async Task DeleteNotificationAsync(NotificationDTO notification)
        {
            var Deletenotification = _mapper.Map<Notification>(notification);
            notificationRepository.Remove(Deletenotification);
            _unitOfWork.Save();
        }
        public async Task UpdateNotificationAsync(UpdateNotificationDTO Updatenotification)
        {
            var notification = _mapper.Map<Notification>(Updatenotification);
            notificationRepository.Update(notification);
            _unitOfWork.Save();
        }
        public async Task<NotificationDTO> GetNotificationByIdAsync(Guid id)
        {
            var notification = notificationRepository.GetById(id);

            if (notification == null)
            {
                throw new Exception($"Notification with ID {id} not found");
            }
            return _mapper.Map<NotificationDTO>(notification);
        }
        public IEnumerable<NotificationDTO> GetNotificationsForUser(Guid receiverId)
        {
            var notifications = notificationRepository.GetNotificationsUser(receiverId);

            if (notifications == null)
            {
                throw new Exception($"Notifications for this User {receiverId} not found");
            }
            return _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
         
        }
    }  
}
