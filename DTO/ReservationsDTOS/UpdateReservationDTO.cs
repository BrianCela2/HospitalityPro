using DTO.ReservationRoomDTOs;
using DTO.ReservationServiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationsDTOS
{
	public class UpdateReservationDTO
	{
		public Guid ReservationId { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime? ReservationDate { get; set; }
		public ICollection<ReservationRoomDTO> ReservationRooms { get; set; }
		public ICollection<CreateReservationServiceDTO> Services { get; set; }
	}
}
