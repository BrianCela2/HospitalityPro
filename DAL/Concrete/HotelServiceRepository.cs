﻿using DAL.Contracts;
using DAL.UoW;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    internal class HotelServiceRepository : BaseRepository<HotelService,Guid> ,IHotelServiceRepository
    {
        public HotelServiceRepository(HospitalityProContext dbContext) : base(dbContext)
        {
        }

        // 
        public IEnumerable<HotelService> GetServicesOfReservation(Guid reservationId)
        {
            return context.Include(x => x.ReservationServices).Where(x => x.ReservationServices.Any(p => p.ReservationId == reservationId)).ToList();
        }
        public int GetServiceUsageCount(Guid serviceId)
        {
            return context.Count(s => s.ServiceId == serviceId);
        }

    }
}
