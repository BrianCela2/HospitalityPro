using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            ReservationRooms = new HashSet<ReservationRoom>();
            Services = new HashSet<HotelService>();
        }

        public Guid ReservationId { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string? ReservationStatus { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }

        public virtual ICollection<HotelService> Services { get; set; }
    }
}
