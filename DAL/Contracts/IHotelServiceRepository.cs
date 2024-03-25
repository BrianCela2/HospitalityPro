using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace DAL.Contracts
{
    public interface IHotelServiceRepository
    {
        Task<IEnumerable<HotelService>> GetAllHotelServicesAsync();
        Task<HotelService> GetHotelServiceByIdAsync(Guid id);
        Task AddHotelServiceAsync(HotelService hotelService);
        Task UpdateHotelServiceAsync(HotelService hotelService);
        Task DeleteHotelServiceAsync(Guid id);
    }
}
