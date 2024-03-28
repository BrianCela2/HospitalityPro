using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationServiceDTOs
{
	public class ReservationServiceDTO
	{
		public Guid ReservationId { get; set; }
		public Guid ServiceId { get; set; }
		public DateTime DateOfPurchase { get; set; }
	}
}
