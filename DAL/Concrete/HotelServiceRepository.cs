using DAL.Contracts;
using DAL.UoW;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    internal class HotelServiceRepository : IHotelServiceRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelServiceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HotelService>> GetAllHotelServicesAsync()
        {
            return await _unitOfWork.GetRepository<HotelService, Guid>().GetAllAsync();
        }

        public async Task<HotelService?> GetHotelServiceByIdAsync(Guid id)
        {
            return await _unitOfWork.GetRepository<HotelService, Guid>().GetByIdAsync(id);
        }

        public async Task AddHotelServiceAsync(HotelService hotelService)
        {
            await _unitOfWork.GetRepository<HotelService, Guid>().AddAsync(hotelService);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateHotelServiceAsync(HotelService hotelService)
        {
            _unitOfWork.GetRepository<HotelService, Guid>().Update(hotelService);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteHotelServiceAsync(Guid id)
        {
            await _unitOfWork.GetRepository<HotelService, Guid>().DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
