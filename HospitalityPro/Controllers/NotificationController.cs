using Domain.Contracts;
using DTO.NotificationDTOs;
using DTO.RoomDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationDomain _notificationDomain;

        public NotificationController(INotificationDomain notificationDomain)
        {
            _notificationDomain = notificationDomain;
        }

        [HttpPost]
        public async Task<IActionResult> AddNotification(CreateNotificationDTO Createnotification)
        {
            if (ModelState.IsValid)
            {
                if (Createnotification == null) { return NotFound(); }
                await _notificationDomain.AddNotificationAsync(Createnotification);

                return NoContent();
            }
            else
            {
                throw new Exception();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(Guid id, UpdateNotificationDTO updatenotification)
        {
            if (ModelState.IsValid)
            {
                if (updatenotification == null && updatenotification.NotificationId != id) { return NotFound(); }
                await _notificationDomain.UpdateNotificationAsync(updatenotification);

                return NoContent();
            }
            else
            {
                throw new Exception();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            if (ModelState.IsValid)
            {
                var notification = await _notificationDomain.GetNotificationByIdAsync(id);
                if (notification == null) { return NotFound(); }
                await _notificationDomain.DeleteNotificationAsync(notification);

                return NoContent();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
