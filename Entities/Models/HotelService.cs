using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class HotelService
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Category { get; set; }
        public string? OpenTime { get; set; }
        public List<Reservation>? Reservations { get; set; }
    }
}
