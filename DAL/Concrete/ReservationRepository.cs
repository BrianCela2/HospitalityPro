using DAL.Contracts;
using Entities.Models;
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

        // 
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
