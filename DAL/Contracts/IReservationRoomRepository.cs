using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
	public interface IReservationRoomRepository : IRepository<ReservationRoom, Guid>
	{
		IEnumerable<ReservationRoom> GetReservationRoomsById(Guid roomId);
		IEnumerable<ReservationRoom> GetRoomsByReservationId(Guid reservationId);
		IEnumerable<ReservationRoom> GetReservationRoomsByIdExcludingCurrentReservation(Guid roomId, Guid reservationIdToExclude);
		IEnumerable<ReservationRoom> GetReservationByRoom(Guid roomId);
        IEnumerable<ReservationRoom> GetRoomReservation(Guid reservationId);
        public int GetRoomOccupancyWithinDateRange(Guid roomId, DateTime startDate, DateTime endDate);

        public IEnumerable<ReservationRoom> GetRoomReservationsWithinDateRange(DateTime startDate, DateTime endDate);
    }
}
