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
            return context.Include(x => x.ReservationServices).Include(x => x.ReservationRooms).Where(x => x.UserId == userID).ToList();
        }
    }
}
