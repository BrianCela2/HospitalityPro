using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace DAL.Contracts
{
    public interface IHotelServiceRepository:IRepository<HotelService, Guid>
    {
        //
        IEnumerable<HotelService> GetServicesOfReservation(Guid reservationId);
        public int GetServiceUsageCount(Guid serviceId);
    }
}
