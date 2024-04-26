using DTO.NotificationDTOs;
using DTO.RoomDTOs;
using Entities.Models;

namespace Domain.Contracts
{
    public interface INotificationDomain
    {
        Task AddNotificationAsync(CreateNotificationDTO notification);
        Task<NotificationDTO> GetNotificationByIdAsync(Guid id);
        IEnumerable<NotificationDTO> GetNotificationsForUser(Guid receiverId);
        Task AddNotificationsAllUserAsync(CreateNotificationDTO Createnotification);
        Task UpdateNotificationToSeen(string userId);

    }
}
