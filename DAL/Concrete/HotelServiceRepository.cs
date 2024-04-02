using DAL.Contracts;
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
        public int GetServiceUsageCount(Guid serviceId)
        {
            return context.Count(s => s.ServiceId == serviceId);
        }

    }
}
