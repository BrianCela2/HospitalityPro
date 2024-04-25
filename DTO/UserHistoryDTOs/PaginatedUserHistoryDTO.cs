using DTO.RoomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserHistoryDTOs
{
	public class PaginatedUserHistoryDTO
	{
		public IEnumerable<UserHistoryDTO> UserHistory { get; set; } = null!;
		public int TotalPages { get; set; }
	}
}
