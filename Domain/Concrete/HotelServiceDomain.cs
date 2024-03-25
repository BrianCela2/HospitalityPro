using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Contracts;
using Domain.Contracts;
using DTO.HotelServiceDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.Concrete
{
    internal class HotelServiceDomain : DomainBase,IHotelServiceDomain
    {

        public HotelServiceDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
        }
        private IHotelServiceRepository hotelServiceRepository => _unitOfWork.GetRepository<IHotelServiceRepository>();

        public async Task<IEnumerable<HotelServiceDTO>> GetAllHotelServicesAsync()
        {
            var hotelServices =  hotelServiceRepository.GetAll();
            return _mapper.Map<IEnumerable<HotelServiceDTO>>(hotelServices);
        }

        public async Task<HotelServiceDTO> GetHotelServiceByIdAsync(Guid id)
        {
            var hotelService =  hotelServiceRepository.GetById(id);
            return _mapper.Map<HotelServiceDTO>(hotelService);
        }

        public async Task AddHotelServiceAsync(CreateHotelServiceDTO hotelServiceDTO)
        {
            var hotelService = _mapper.Map<HotelService>(hotelServiceDTO);
            hotelServiceRepository.Add(hotelService);
            _unitOfWork.Save();
        }

        public async Task UpdateHotelServiceAsync(UpdateHotelServiceDTO hotelServiceDTO)
        {
            var hotelService = _mapper.Map<HotelService>(hotelServiceDTO);
            hotelServiceRepository.Update(hotelService);
            _unitOfWork.Save();

        }

        public async Task DeleteHotelServiceAsync(Guid id)
        {
            hotelServiceRepository.Remove(id);
            _unitOfWork.Save();

        }
    }
}
