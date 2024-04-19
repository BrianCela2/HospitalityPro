using DTO.ReservationRoomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationsDTOS
{
	public class PaginatedReservationDTO
	{
		public IEnumerable<ReservationDTO> Reservations { get; set; } = null!;
		public int TotalPages { get; set; }

	}
}
