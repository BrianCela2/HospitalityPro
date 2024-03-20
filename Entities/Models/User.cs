using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class User
    {
        public User()
        {
            NotificationReceivers = new HashSet<Notification>();
            NotificationSenders = new HashSet<Notification>();
            Reservations = new HashSet<Reservation>();
            UserRoles = new HashSet<UserRole>();
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? Birthday { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Notification> NotificationReceivers { get; set; }
        public virtual ICollection<Notification> NotificationSenders { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
