using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IReservationServiceRepository : IRepository<ReservationService, Guid>
    {
        IEnumerable<ReservationService> GetReservationServicesByReservationId(Guid reservationId);
        IEnumerable<ReservationService> GetReservationServicesByServiceId(Guid serviceId);
    }
}
