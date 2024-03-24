
using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.ReservationsDTOS;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
	internal class ReservationDomain :DomainBase, IReservationDomain
	{
		public ReservationDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
		{
		}

		private IReservationRepository reservationRepository => _unitOfWork.GetRepository<IReservationRepository>();

		public async Task AddReservationAsync(ReservationDTO reservationDto)
		{
			var reservation = _mapper.Map<Reservation>(reservationDto);
			reservationRepository.Add(reservation);
			_unitOfWork.Save();
		}

		public async Task<IEnumerable<ReservationDTO>> GetAllReservationsAsync()
		{
			IEnumerable<Reservation> reservations = reservationRepository.GetAll();

			var mapped = _mapper.Map<IEnumerable<ReservationDTO>>(reservations);

			return mapped;
		}

		public async Task<ReservationDTO> GetReservationByIdAsync(Guid id)
		{
			Reservation reservation = reservationRepository.GetById(id);

			ReservationDTO mapped = _mapper.Map<ReservationDTO>(reservation);

			return mapped;
		}
	}
}
