using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.RoomDTOs
{
	public class PaginatedRoomDTO
	{
		public IEnumerable<RoomDTO> Rooms { get; set; } = null!;
		public int TotalPages { get; set; }
	}
}
