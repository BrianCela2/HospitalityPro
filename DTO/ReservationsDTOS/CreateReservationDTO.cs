using DTO.ReservationRoomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationsDTOS
{
	public class CreateReservationDTO
	{
		public Guid UserId { get; set; }
		public DateTime? ReservationDate { get; set; }
		public int? ReservationStatus { get; set; }
		public List<ReservationRoomDTO>? Rooms { get; set; }
		public List<Guid>? ServiceId { get; set; }

	}
}
