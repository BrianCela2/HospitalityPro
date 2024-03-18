using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    public partial class ReservationRoom
    {
        [Key]
        [Column("ReservationID")]
        public Guid ReservationId { get; set; }
        [Key]
        [Column("RoomID")]
        public Guid RoomId { get; set; }
        [Column(TypeName = "date")]
        public DateTime CheckInDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime CheckOutDate { get; set; }

        [ForeignKey("ReservationId")]
        [InverseProperty("ReservationRooms")]
        public virtual Reservation Reservation { get; set; } = null!;
        [ForeignKey("RoomId")]
        [InverseProperty("ReservationRooms")]
        public virtual Room Room { get; set; } = null!;
    }
}
