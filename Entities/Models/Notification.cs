using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid? SenderId { get; set; }
        public DateTime? SendDateTime { get; set; }
        public string? MessageContent { get; set; }
        public bool? IsSeen { get; set; }
        public Guid? ContentId { get; set; }

        public virtual User Receiver { get; set; } = null!;
        public virtual User? Sender { get; set; }
    }
}
