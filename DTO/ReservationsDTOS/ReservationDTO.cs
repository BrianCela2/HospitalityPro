using DTO.HotelServiceDTOs;
using DTO.ReservationRoomDTOs;
using DTO.ReservationServiceDTOs;
using DTO.UserDTO;
using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationsDTOS
{
	public class ReservationDTO
	{
		public Guid ReservationId { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime? ReservationDate { get; set; }
		public ReservationStatusEnum ReservationStatus { get; set; }
		public virtual Guid UserId { get; set; }
		public virtual ICollection<ReservationRoomDTO> ReservationRooms { get; set; } = null!;
		public virtual ICollection<CreateReservationServiceDTO>? Services { get; set; }
	}
}
