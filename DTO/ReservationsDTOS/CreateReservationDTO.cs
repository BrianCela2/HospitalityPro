using DTO.HotelServiceDTOs;
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
		public DateTime? ReservationDate { get; set; }
		public int? ReservationStatus { get; set; }
		public List<ReservationRoomDTO> Rooms { get; set; } = null!;
		public List<HotelServiceDTO>? HotelService { get; set; }	

	}
}
