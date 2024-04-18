
using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.HotelServiceDTOs;
using DTO.UserDTO;
using Entities.Models;
using LamarCodeGeneration.Frames;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace Domain.Concrete
{
    internal class HotelServiceDomain : DomainBase,IHotelServiceDomain
    {
		private readonly PaginationHelper<HotelService> _paginationHelper;
		public HotelServiceDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
			_paginationHelper = new PaginationHelper<HotelService>();
		}
        private IHotelServiceRepository hotelServiceRepository => _unitOfWork.GetRepository<IHotelServiceRepository>();
        private IReservationServiceRepository reservationServiceRepository => _unitOfWork.GetRepository<IReservationServiceRepository>();
        public async Task<IEnumerable<HotelServiceDTO>> GetAllHotelServicesAsync(int page, int pageSize, string sortField, string sortOrder, string searchString)
		{
				searchString = searchString?.ToLower();
				IEnumerable<HotelService> hotelServices = hotelServiceRepository.GetAll();
				Func<HotelService, bool> filterFunc = u => string.IsNullOrEmpty(searchString) || u.ServiceName.ToLower().Contains(searchString);
				IEnumerable<HotelService> paginatedHotelServices = _paginationHelper.GetPaginatedData(hotelServices, page, pageSize, sortField, sortOrder, searchString, filterFunc: filterFunc);
				return _mapper.Map<IEnumerable<HotelServiceDTO>>(paginatedHotelServices);
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
            var hotelService = reservationServiceRepository.GetReservationServicesByServiceId(id);
            reservationServiceRepository.RemoveRange(hotelService);
            hotelServiceRepository.Remove(id);
            _unitOfWork.Save();

        }
        public IEnumerable<HotelServiceDTO> GetServiceReservation(Guid reservationId)
        {
            var hotelServices = hotelServiceRepository.GetServicesOfReservation(reservationId);
            var hotelServicesDTO = _mapper.Map<IEnumerable<HotelServiceDTO>>(hotelServices);
            return hotelServicesDTO;
        }
        //
        public int GetServiceUsageCount(Guid serviceId)
        {
            return hotelServiceRepository.GetServiceUsageCount(serviceId);
        }
    }
}
