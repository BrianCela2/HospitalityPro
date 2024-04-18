using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.HotelServiceDTOs;

namespace Domain.Contracts
{
    public interface IHotelServiceDomain {
        Task AddHotelServiceAsync(CreateHotelServiceDTO hotelServiceDTO);
        Task<HotelServiceDTO> GetHotelServiceByIdAsync(Guid id);
        Task<IEnumerable<HotelServiceDTO>> GetAllHotelServicesAsync(int page, int pageSize, string sortField, string sortOrder, string searchString);
        Task UpdateHotelServiceAsync(UpdateHotelServiceDTO hotelServiceDTO);
        Task DeleteHotelServiceAsync(Guid id);
        IEnumerable<HotelServiceDTO> GetServiceReservation(Guid reservationId);

        //
        public int GetServiceUsageCount(Guid serviceId);
    }
}
