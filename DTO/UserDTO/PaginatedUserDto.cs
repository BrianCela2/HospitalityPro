using DTO.ReservationsDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserDTO
{
	public class PaginatedUserDto
	{
		public IEnumerable<UserDTO> Users { get; set; } = null!;
		public int TotalPages { get; set; }
	}
}
