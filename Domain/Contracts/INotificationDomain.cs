using DTO.NotificationDTOs;
using DTO.RoomDTOs;
using Entities.Models;

namespace Domain.Contracts
{
    public interface INotificationDomain
    {
        Task AddNotificationAsync(CreateNotificationDTO notification);
        Task DeleteNotificationAsync(NotificationDTO notification);
        Task UpdateNotificationAsync(UpdateNotificationDTO notification);
        Task<NotificationDTO> GetNotificationByIdAsync(Guid id);
        IEnumerable<NotificationDTO> GetNotificationsForUser(Guid receiverId);
        Task AddNotificationsAllUserAsync(CreateNotificationDTO Createnotification);

        IEnumerable<UpdateNotificationDTO> NotificationsUnSeen();

    }
}
