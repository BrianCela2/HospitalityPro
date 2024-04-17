using DTO.ReservationRoomDTOs;
using DTO.ReservationServiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO.ReservationsDTOS
{
	public class UpdateReservationDTO
	{
		public Guid ReservationId { get; set; }
		[JsonIgnore]
		public decimal TotalPrice { get; set; }
		[JsonIgnore]

		public int ReservationStatus { get; set; }
		public DateTime? ReservationDate { get; set; }
		public ICollection<ReservationRoomDTO> ReservationRooms { get; set; }
		public ICollection<ReservationServiceDTO> ReservationServices { get; set; }
	}
}
