using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            ReservationRooms = new HashSet<ReservationRoom>();
            ReservationServices = new HashSet<ReservationService>();
        }

        public Guid ReservationId { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public int? ReservationStatus { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }
        public virtual ICollection<ReservationService> ReservationServices { get; set; }
    }
}
