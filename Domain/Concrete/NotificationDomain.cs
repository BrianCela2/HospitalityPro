﻿using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using Domain.Notifications;
using DTO.NotificationDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Helpers.StaticFunc;
namespace Domain.Concrete
{
    internal class NotificationDomain : DomainBase, INotificationDomain
    {
        private  IHubContext<NotificationHub,INotificationHub> _notificationHubContext;

        public NotificationDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub, INotificationHub> notificationHubContext) : base(unitOfWork, mapper, httpContextAccessor)
        {
            _notificationHubContext = notificationHubContext;
        }
        private INotificationRepository notificationRepository => _unitOfWork.GetRepository<INotificationRepository>();
        private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();
        public async Task AddNotificationAsync(CreateNotificationDTO Createnotification)
        {
            var notification = _mapper.Map<Notification>(Createnotification);
            notificationRepository.Add(notification);
            _unitOfWork.Save();
        }
        public async Task AddNotificationsAllUserAsync(CreateNotificationDTO Createnotification)
        {
            IEnumerable<User> users = userRepository.GetAll();
            List<Notification> notifications = new List<Notification>();
            var notificationMessage =new Notification { };
            var receiverIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            Guid userId;
            if (receiverIdClaim != null)
            {
                userId = StaticFunc.ConvertGuid(receiverIdClaim);
            }
            else
            {
                throw new Exception("User doesn't not exist");
            }
            foreach (var user in users)
            {
                var notification = _mapper.Map<Notification>(Createnotification);
                notificationMessage = notification;
                notification.ReceiverId = user.UserId;
                notification.SenderId = userId;
                notifications.Add(notification);
            }

            notificationRepository.AddRange(notifications);
            await _notificationHubContext.Clients.All.SendNotificationAll(notificationMessage);
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
        public async Task UpdateNotificationToSeen(string userId)
        {
            var notifications = notificationRepository.GetNotificationsUnSeen(userId);

            foreach (var notification in notifications){
                notification.IsSeen = true;
            }
            notificationRepository.UpdateRange(notifications);
            _unitOfWork.Save();

        }
    }  
}
