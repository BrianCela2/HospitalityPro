using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    [Index("Email", Name = "UQ__Users__A9D10534A61E4554", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            NotificationReceivers = new HashSet<Notification>();
            NotificationSenders = new HashSet<Notification>();
            Reservations = new HashSet<Reservation>();
            UserRoles = new HashSet<UserRole>();
        }

        [Key]
        [Column("UserID")]
        public Guid UserId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string Country { get; set; } = null!;
        [StringLength(30)]
        [Unicode(false)]
        public string City { get; set; } = null!;
        public bool? IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [InverseProperty("Receiver")]
        public virtual ICollection<Notification> NotificationReceivers { get; set; }
        [InverseProperty("Sender")]
        public virtual ICollection<Notification> NotificationSenders { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Reservation> Reservations { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
