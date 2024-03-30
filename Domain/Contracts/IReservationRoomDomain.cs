using DTO.ReservationRoomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
	public interface IReservationRoomDomain
	{
		Task <IEnumerable<ReservationRoomDTO>> GetAllReservationRoomsAsync();
		Task<IEnumerable<ReservationRoomDTO>> GetReservationsRoomByRoomId(Guid roomId);

		//
		public int GetRoomOccupancyWithinDateRange(Guid roomId, DateTime startDate, DateTime endDate);
		public IEnumerable<ReservationRoomDTO> GetRoomReservationsWithinDateRange(DateTime startDate, DateTime endDate);

    }
}
