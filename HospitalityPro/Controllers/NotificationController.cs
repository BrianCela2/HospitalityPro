using Domain.Contracts;
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
        public async Task<IActionResult> AddNotification(Notification notification)
        {
            if (notification == null) { return NotFound(); }
            await _notificationDomain.AddNotificationAsync(notification);
            return NoContent();
        }
    }
}
