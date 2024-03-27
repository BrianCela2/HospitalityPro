
using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.HotelServiceDTOs;
using DTO.ReservationRoomDTOs;
using DTO.ReservationsDTOS;
using Entities.Models;
using Helpers.StaticFunc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private IRoomRepository roomRepository => _unitOfWork.GetRepository<IRoomRepository>();
		private IReservationRoomRepository reservationRoomRepository => _unitOfWork.GetRepository<IReservationRoomRepository>();

		public async Task AddReservationAsync(CreateReservationDTO reservationDto)
		{

			var receiverIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

			Guid userId = StaticFunc.ConvertGuid(receiverIdClaim);
			if (userId == null) throw new Exception("User not found");
			var reservation = _mapper.Map<Reservation>(reservationDto);
			reservation.UserId = userId;
			decimal Price = 0;
			foreach(var roomReser in reservationDto.ReservationRooms)
			{
				var room = roomRepository.GetById(roomReser.RoomId);
				Price = room.Price;
			}
			reservation.TotalPrice = Price;
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

		public async Task UpdateReservation(UpdateReservationDTO updateReservationDTO)
		{
			Reservation reservation =  reservationRepository.GetById(updateReservationDTO.ReservationId);

			if (reservation == null)
			{
				throw new Exception("Reservation not found");
			}
			var mappedReservationRooms = _mapper.Map<ICollection<ReservationRoom>>(updateReservationDTO.ReservationRooms);
			foreach(var reservationRoom in mappedReservationRooms)
			{
				reservationRoom.ReservationId = updateReservationDTO.ReservationId;
				reservationRoomRepository.Update(reservationRoom);
				_unitOfWork.Save();
			}

			var mappedReservation = _mapper.Map<Reservation>(updateReservationDTO);
			reservationRepository.Update(mappedReservation);

			_unitOfWork.Save();
		}

	}
}
