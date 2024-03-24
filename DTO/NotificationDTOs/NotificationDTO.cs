using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NotificationDTOs
{
    public record NotificationDTO
    {
            public Guid NotificationId { get; set; }
            public Guid ReceiverId { get; set; }
            public Guid? SenderId { get; set; }
            public DateTime? SendDateTime { get; set; }
            public string MessageContent { get; set; } = null!;
            public bool? IsSeen { get; set; }
            public DTO.UserDTO.UserDTO Receiver { get; set; } = null!;
            public DTO.UserDTO.UserDTO? Sender { get; set; } 

    }
}
