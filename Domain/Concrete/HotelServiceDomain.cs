using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Contracts;
using Domain.Contracts;
using DTO.HotelServiceDTOs;
using Entities.Models;

namespace Domain.Concrete
{
    public class HotelServiceDomain : IHotelServiceDomain
    {
        private readonly IHotelServiceRepository _hotelServiceRepository;
        private readonly IMapper _mapper;

        public HotelServiceDomain(IHotelServiceRepository hotelServiceRepository, IMapper mapper)
        {
            _hotelServiceRepository = hotelServiceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HotelServiceDTO>> GetAllHotelServicesAsync()
        {
            var hotelServices = await _hotelServiceRepository.GetAllHotelServicesAsync();
            return _mapper.Map<IEnumerable<HotelServiceDTO>>(hotelServices);
        }

        public async Task<HotelServiceDTO> GetHotelServiceByIdAsync(Guid id)
        {
            var hotelService = await _hotelServiceRepository.GetHotelServiceByIdAsync(id);
            return _mapper.Map<HotelServiceDTO>(hotelService);
        }

        public async Task AddHotelServiceAsync(CreateHotelServiceDTO hotelServiceDTO)
        {
            var hotelService = _mapper.Map<HotelService>(hotelServiceDTO);
            await _hotelServiceRepository.AddHotelServiceAsync(hotelService);
        }

        public async Task UpdateHotelServiceAsync(UpdateHotelServiceDTO hotelServiceDTO)
        {
            var hotelService = _mapper.Map<HotelService>(hotelServiceDTO);
            await _hotelServiceRepository.UpdateHotelServiceAsync(hotelService);
        }

        public async Task DeleteHotelServiceAsync(Guid id)
        {
            await _hotelServiceRepository.DeleteHotelServiceAsync(id);
        }
    }
}
