using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.ReservationRoomDTOs;
using DTO.RoomDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
	internal class ReservationRoomDomain : DomainBase, IReservationRoomDomain
	{
		public ReservationRoomDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
		{
		}

		private IReservationRoomRepository reservationRoomRepository => _unitOfWork.GetRepository<IReservationRoomRepository>();

		public async Task<IEnumerable<ReservationRoomDTO>> GetAllReservationRoomsAsync()
		{
			IEnumerable<ReservationRoom> rooms = reservationRoomRepository.GetAll();
			var reservationRooms = _mapper.Map<IEnumerable<ReservationRoomDTO>>(rooms);
			return reservationRooms;
		}

		public async Task<IEnumerable<ReservationRoomDTO>> GetReservationsRoomByRoomId(Guid roomId)
		{
			IEnumerable<ReservationRoom> rooms = reservationRoomRepository.GetReservationRoomsById(roomId);
			var reservationRooms = _mapper.Map<IEnumerable<ReservationRoomDTO>>(rooms);
			return reservationRooms;
		}

        //
        public int GetRoomOccupancyWithinDateRange(Guid roomId, DateTime startDate, DateTime endDate)
        {
            return reservationRoomRepository.GetRoomOccupancyWithinDateRange(roomId, startDate, endDate);
        }

        ////
        public IEnumerable<ReservationRoomDTO> GetRoomReservationsWithinDateRange(DateTime startDate, DateTime endDate)
        {
            var reservations = reservationRoomRepository.GetRoomReservationsWithinDateRange(startDate, endDate);
            return _mapper.Map<IEnumerable<ReservationRoomDTO>>(reservations);
        }


    }
}
