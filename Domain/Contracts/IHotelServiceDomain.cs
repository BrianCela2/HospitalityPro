﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.HotelServiceDTOs;

namespace Domain.Contracts
{
    public interface IHotelServiceDomain {
        Task AddHotelServiceAsync(CreateHotelServiceDTO hotelServiceDTO);
        Task<HotelServiceDTO> GetHotelServiceByIdAsync(Guid id);
        Task<IEnumerable<HotelServiceDTO>> GetAllHotelServicesAsync();
        Task UpdateHotelServiceAsync(UpdateHotelServiceDTO hotelServiceDTO);
        Task DeleteHotelServiceAsync(Guid id);
    }
}
