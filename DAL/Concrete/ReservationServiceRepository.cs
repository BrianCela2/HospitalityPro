using DAL.Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    internal class ReservationServiceRepository : BaseRepository<ReservationService, Guid>, IReservationServiceRepository
    {

        public ReservationServiceRepository(HospitalityProContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<ReservationService> GetReservationServicesByReservationId(Guid reservationId)
        {
            return context.Where(x=>x.ReservationId==reservationId).ToList();
        }

    }
}
