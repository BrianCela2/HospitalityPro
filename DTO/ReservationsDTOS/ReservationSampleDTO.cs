using DTO.ReservationRoomDTOs;
using DTO.ReservationServiceDTOs;
using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationsDTOS
{
	public class ReservationSampleDTO
	{
		public ReservationStatusEnum ReservationStatus { get; set; }
		public DateTime? ReservationDate { get; set; }
		public virtual ICollection<ReservationRoomDTO> ReservationRooms { get; set; } = null!;
		public virtual ICollection<CreateReservationServiceDTO>? ReservationServices { get; set; }
	}
}
