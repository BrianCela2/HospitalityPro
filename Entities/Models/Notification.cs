using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    public partial class Notification
    {
        [Key]
        [Column("NotificationID")]
        public Guid NotificationId { get; set; }
        [Column("ReceiverID")]
        public Guid ReceiverId { get; set; }
        [Column("SenderID")]
        public Guid? SenderId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SendDateTime { get; set; }
        [Unicode(false)]
        public string? MessageContent { get; set; }
        [Column("isSeen")]
        public bool? IsSeen { get; set; }

        [ForeignKey("ReceiverId")]
        [InverseProperty("NotificationReceivers")]
        public virtual User Receiver { get; set; } = null!;
        [ForeignKey("SenderId")]
        [InverseProperty("NotificationSenders")]
        public virtual User? Sender { get; set; }
    }
}
