using DTO.ReservationsDTOS;
using DTO.RoomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationRoomDTOs
{
    public record ReservationRoomDetailsDTO
    {
        public Guid ReservationID { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public virtual ReservationDTO Reservation { get; set; } = null!;
        public virtual RoomDTO Room { get; set; } = null!;

    }
}
