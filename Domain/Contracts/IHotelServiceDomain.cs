using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.HotelServiceDTOs;

namespace Domain.Contracts
{
    public interface IHotelServiceDomain
    {
        Task<IEnumerable<HotelServiceDTO>> GetAllHotelServicesAsync();
        Task<HotelServiceDTO> GetHotelServiceByIdAsync(Guid id);
        Task AddHotelServiceAsync(CreateHotelServiceDTO hotelServiceDTO);
        Task UpdateHotelServiceAsync(UpdateHotelServiceDTO hotelServiceDTO);
        Task DeleteHotelServiceAsync(Guid id);
    }
}
