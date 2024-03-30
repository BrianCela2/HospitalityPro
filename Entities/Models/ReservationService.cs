using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class ReservationService
    {
        public Guid ReservationId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime? DateOfPurchase { get; set; }

        public virtual Reservation Reservation { get; set; } = null!;
        public virtual HotelService Service { get; set; } = null!;
    }
}
