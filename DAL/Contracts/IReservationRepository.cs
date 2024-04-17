using Entities.Models;

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
