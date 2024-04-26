using DAL.Contracts;
using Domain.Contracts;
using DTO.NotificationDTOs;
using DTO.RoomDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace HospitalityPro.Controllers
{
    [Route("[controller]")]
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
        [HttpPost]
        [Route("AddNotificationAllUsers")]
        public async Task<IActionResult> AddNotifications(CreateNotificationDTO Createnotification)
        {
            if (ModelState.IsValid)
            {
                if (Createnotification == null) { return NotFound(); }
                await _notificationDomain.AddNotificationsAllUserAsync(Createnotification);

                return NoContent();
            }
            else
            {
                throw new Exception();
            }
        }
       
        [HttpGet("{receiverId}")]

        public IActionResult GetNotificationsReceiver(Guid receiverId)
        {
            
                var notifications =  _notificationDomain.GetNotificationsForUser(receiverId);
                if (notifications == null)
                {
                    return NotFound();
                }
                return Ok(notifications);
        }
        [HttpPut("NotificationsSeen/{userId}")]
        public async Task<IActionResult> ChangeNotificationToSeen(string userId)
        {
            if (userId != null)
            {
                await _notificationDomain.UpdateNotificationToSeen(userId);
                return NoContent();
            }
            else
            {
                throw new Exception();
            }
        }

    }
}
