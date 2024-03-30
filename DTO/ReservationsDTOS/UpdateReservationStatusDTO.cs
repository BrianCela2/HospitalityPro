using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationsDTOS
{
	public class UpdateReservationStatusDTO
	{
		public Guid ReservationId { get; set; }
		public int ReservationStatus { get; set; }
	}
}
