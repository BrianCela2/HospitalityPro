using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
	public interface IReservationRepository :IRepository<Reservation, Guid>
	{
        Reservation GetReservation(Guid reservationId);
        IEnumerable<Reservation> GetReservationsOfUser(Guid userID);
        IEnumerable<Reservation> ReservationsWithRoomServices();
        Guid GetUserIdByReservation(Guid reservationId);
    
        public int GetStaysCountWithinDateRange(DateTime startDate, DateTime endDate);
        public decimal GetTotalRevenueWithinDateRange(DateTime startDate, DateTime endDate);

    }
}
