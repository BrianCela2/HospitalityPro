using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class ReservationRoom
    {
        public Guid ReservationId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public virtual Reservation Reservation { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
    }
}
