using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            ReservationRooms = new HashSet<ReservationRoom>();
            Services = new HashSet<HotelService>();
        }

        [Key]
        [Column("ReservationID")]
        public Guid ReservationId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ReservationDate { get; set; }
        public int? ReservationStatus { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Reservations")]
        public virtual User? User { get; set; }
        [InverseProperty("Reservation")]
        public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }

        [ForeignKey("ReservationId")]
        [InverseProperty("Reservations")]
        public virtual ICollection<HotelService> Services { get; set; }
    }
}
