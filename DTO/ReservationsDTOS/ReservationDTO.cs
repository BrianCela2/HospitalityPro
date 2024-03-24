using DTO.HotelServiceDTO;
using DTO.ReservationRoomDTOs;
using DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationsDTOS
{
	public class ReservationDTO
	{
		public decimal TotalPrice { get; set; }
		public DateTime? ReservationDate { get; set; }
		public virtual UserDTO.UserDTO? User { get; set; }
		public virtual ICollection<ReservationRoomDTO> ReservationRooms { get; set; }

		public virtual ICollection<HotelServiceDTO.HotelServiceDTO> Services { get; set; }
	}
}
