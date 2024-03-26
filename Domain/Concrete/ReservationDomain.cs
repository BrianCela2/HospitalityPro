
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
		private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();
		private IReservationRoomRepository reservationRoomRepository => _unitOfWork.GetRepository<IReservationRoomRepository>();

		public async Task AddReservationAsync(CreateReservationDTO reservationDto)
		{
			var user = userRepository.GetById(reservationDto.UserId);
			if (user == null) throw new Exception("User not found");
			var reservation = _mapper.Map<Reservation>(reservationDto);

			reservationRepository.Add(reservation);
			_unitOfWork.Save();
			foreach (var roomDto in reservationDto.Rooms)
			{
				var roomReservations = reservationRoomRepository.GetReservationRoomsById(roomDto.RoomId);
				foreach (var roomReservation in roomReservations)
				{
					if (roomReservation.CheckInDate < roomDto.CheckOutDate && roomReservation.CheckOutDate > roomDto.CheckInDate)
					{
						throw new Exception($"Room with ID {roomDto.RoomId} is not available for the specified dates.");
					}
				}
				var reservationRoom = _mapper.Map<ReservationRoom>(roomDto);
				reservationRoom.ReservationId = reservation.ReservationId;
				reservationRoomRepository.Add(reservationRoom);
				_unitOfWork.Save();
			}

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
