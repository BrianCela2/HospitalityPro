using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.ReservationServiceDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.Concrete
{
    internal class ReservationServiceDomain : DomainBase, IReservationServiceDomain
	{
		public ReservationServiceDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
		{
		}
		private IReservationServiceRepository reservationServiceRepository => _unitOfWork.GetRepository<IReservationServiceRepository>();

		public async Task AddReservationServiceAsync(List<ReservationServiceDTO> reservationServiceDto)
		{
			var mappedServices = _mapper.Map<IEnumerable<ReservationService>>(reservationServiceDto);
		    reservationServiceRepository.AddRange(mappedServices);
			_unitOfWork.Save(); 
		}
	}
}
