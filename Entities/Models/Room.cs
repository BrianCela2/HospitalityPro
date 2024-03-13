using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Room
    {
        public Room()
        {
            ReservationRooms = new HashSet<ReservationRoom>();
            RoomPhotos = new HashSet<RoomPhoto>();
        }

        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int? Capacity { get; set; }
        public decimal Price { get; set; }
        public string? RoomStatus { get; set; }
        public string? Category { get; set; }

        public virtual ICollection<ReservationRoom> ReservationRooms { get; set; }
        public virtual ICollection<RoomPhoto> RoomPhotos { get; set; }
    }
}
