using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace DAL.Contracts
{
    public interface IHotelServiceRepository:IRepository<HotelService, Guid>
    {
      
    }
}
