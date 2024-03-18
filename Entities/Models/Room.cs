using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    public partial class Room
    {
        public Room()
        {
            ReservationRooms = new HashSet<ReservationRoom>();
            RoomPhotos = new HashSet<RoomPhoto>();
        }

        [Key]
        [Column("RoomID")]
        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int? Capacity { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public int? RoomStatus { get; set; }
        public int? Category { get; set; }

        [InverseProperty("Room")]
        public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }
        [InverseProperty("Room")]
        public virtual ICollection<RoomPhoto> RoomPhotos { get; set; }
    }
}
