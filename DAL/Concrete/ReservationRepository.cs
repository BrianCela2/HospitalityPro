using DAL.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
	internal class ReservationRepository : BaseRepository<Reservation, Guid>, IReservationRepository
	{
        public ReservationRepository(HospitalityProContext dbContext) : base(dbContext)
		{
            
        }

        public Reservation GetReservation(Guid reservationId)
        {
            return context.Include(x => x.ReservationServices).Include(x => x.ReservationRooms).Where(x => x.ReservationId == reservationId).FirstOrDefault();
        }

        public IEnumerable<Reservation> GetReservationsOfUser(Guid userID)
        {
            return context.Include(x => x.ReservationServices).Include(x => x.ReservationRooms).Include(x=>x.User).Where(x => x.UserId == userID).ToList();
        }

        public IEnumerable<Reservation> ReservationsWithRoomServices()
        {
            return context.Include(x => x.ReservationServices).Include(x => x.ReservationRooms).Include(x => x.ReservationServices).Include(x=>x.User).ToList();
        }
    
		public Guid GetUserIdByReservation(Guid reservationId)
		{
            return (Guid)context.Where(x => x.ReservationId == reservationId).Select(x => x.UserId).FirstOrDefault();
		}
	 
        public int GetStaysCountWithinDateRange(DateTime startDate, DateTime endDate)
        {
            return context.Count(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate);
        }

        public decimal GetTotalRevenueWithinDateRange(DateTime startDate, DateTime endDate)
        {
            return context.Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate)
                          .Sum(r => r.TotalPrice);
        }


    }
}
