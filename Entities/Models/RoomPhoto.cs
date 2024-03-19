using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class RoomPhoto
    {
        public Guid PhotoId { get; set; }
        public string PhotoPath { get; set; } = null!;
        public Guid? RoomId { get; set; }
        public byte[]? PhotoContent { get; set; }

        public virtual Room? Room { get; set; }
    }
}
