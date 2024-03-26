using DTO.ReservationsDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.HotelServiceDTO
{
	public class HotelServiceDTO
	{
		public string ServiceName { get; set; } = null!;
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int? Category { get; set; }
		public string? OpenTime { get; set; }

		public virtual ICollection<ReservationDTO> Reservations { get; set; }
	}
}
