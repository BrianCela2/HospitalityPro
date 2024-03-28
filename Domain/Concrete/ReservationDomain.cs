using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.ReservationsDTOS;
using DTO.ReservationServiceDTOs;
using Entities.Models;
using Helpers.StaticFunc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
        private IReservationServiceRepository reservationServiceRepository => _unitOfWork.GetRepository<IReservationServiceRepository>();
		private IHotelServiceRepository hotelServiceRepository => _unitOfWork.GetRepository<IHotelServiceRepository>();

        public async Task AddReservationAsync(CreateReservationDTO reservationDto)
		{

			var receiverIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			Guid userId;
			if (receiverIdClaim != null){
				 userId = StaticFunc.ConvertGuid(receiverIdClaim);
			}else{
				throw new Exception("User doesn't not exist");
			}
			
			var reservation = _mapper.Map<Reservation>(reservationDto);
			reservation.UserId = userId;

			decimal Price = 0;
			int Datedifferences = 0;

			foreach(var roomReservation in reservationDto.ReservationRooms){
				var room = roomRepository.GetById(roomReservation.RoomId);
				Price += room.Price;
                var roomAvailable = reservationRoomRepository.GetReservationRoomsById(roomReservation.RoomId);

				foreach(var roomDates in roomAvailable){
					if((roomReservation.CheckInDate >= roomDates.CheckInDate && roomReservation.CheckInDate <=roomDates.CheckOutDate)||(roomReservation.CheckOutDate>roomDates.CheckInDate && roomReservation.CheckOutDate<=roomDates.CheckOutDate)){
						throw new Exception("You Reservation can be done because Room is not available");
					}
                    int differenceOfDays = StaticFunc.GetDayDiff(Datedifferences,roomReservation.CheckInDate,roomReservation.CheckOutDate);
					Datedifferences = differenceOfDays;
                }
            }
			if (reservationDto.ReservationRooms != null)
			{
				foreach (var reservationService in reservationDto.ReservationServices)
				{
					var service = hotelServiceRepository.GetById(reservationService.ServiceId);
					Price += service.Price;
				}
			}
			reservation.TotalPrice = StaticFunc.GetTotalPrice(Datedifferences, Price);
            reservationRepository.Add(reservation);
			_unitOfWork.Save();
		}
		public async Task<IEnumerable<ReservationDTO>> GetAllReservationsAsync()
		{
			IEnumerable<Reservation> reservations = reservationRepository.GetAll();

			var mapped = _mapper.Map<IEnumerable<ReservationDTO>>(reservations);

			return mapped;
		}
        public async Task DeleteReservation(Guid reservationId)
        {
            Reservation reservations = reservationRepository.GetReservation(reservationId);
			reservationRoomRepository.RemoveRange(reservations.ReservationRooms);
			_unitOfWork.Save();

			reservationServiceRepository.RemoveRange(reservations.ReservationServices);
            _unitOfWork.Save();

            if (reservations == null) throw new Exception();
			reservationRepository.Remove(reservations);
			_unitOfWork.Save();
        }
		public async Task AddExtraService(Guid reservationID,Guid serviceID){

			ReservationService createReservationService = new ReservationService { ReservationId=reservationID,ServiceId = serviceID, DateOfPurchase = DateTime.Now };
			Reservation reservation = reservationRepository.GetReservation(reservationID);
			reservation.ReservationServices.Add(createReservationService);

			 HotelService service = hotelServiceRepository.GetById(createReservationService.ServiceId);
			 reservation.TotalPrice += service.Price;

             reservationRepository.Update(reservation);
			_unitOfWork.Save();
		}
        public async Task<ReservationDTO> GetReservationByIdAsync(Guid id)
		{
			Reservation reservation = reservationRepository.GetById(id);
			ReservationDTO mapped = _mapper.Map<ReservationDTO>(reservation);

			return mapped;
		}

		public async Task UpdateReservation(UpdateReservationDTO updateReservationDTO)
		{
			var receiverIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

			Guid userId = StaticFunc.ConvertGuid(receiverIdClaim);
			if (userId == null) throw new Exception("User not found");
			
			Reservation reservation =  reservationRepository.GetById(updateReservationDTO.ReservationId);

			if (reservation == null)
			{
				throw new Exception("Reservation not found");
			}

			foreach(var room in updateReservationDTO.ReservationRooms)
			{
				var roomReservations = reservationRoomRepository.GetReservationRoomsById(room.RoomId);
				foreach(var roomReservation in roomReservations)
				{
					if ((room.CheckInDate < roomReservation.CheckOutDate && room.CheckInDate >= roomReservation.CheckInDate) ||
						(room.CheckOutDate > roomReservation.CheckInDate && room.CheckOutDate <= roomReservation.CheckOutDate))
					{
						throw new Exception($"Room {room.RoomId} is already reserved for the specified dates.");
					}
				}
				var mappedReservationRoom = _mapper.Map<ReservationRoom>(room);
				mappedReservationRoom.ReservationId = updateReservationDTO.ReservationId;
				reservationRoomRepository.Update(mappedReservationRoom);
				_unitOfWork.Save();
			}

			foreach (var service in updateReservationDTO.Services)
			{
				var mappedReservationService = _mapper.Map<ReservationService>(service);
				mappedReservationService.ReservationId = updateReservationDTO.ReservationId;
				reservationServiceRepository.Update(mappedReservationService);
				_unitOfWork.Save();
			}

			var mappedReservation = _mapper.Map<Reservation>(updateReservationDTO);
			mappedReservation.UserId = userId;
			reservationRepository.Update(mappedReservation);
			_unitOfWork.Save();

		}

	}

        public IEnumerable<ReservationDTO> GetReservationsOfUser()
        {

            var receiverIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            Guid userId;
            if (receiverIdClaim != null)
            {
                userId = StaticFunc.ConvertGuid(receiverIdClaim);
            }
            else
            {
                throw new Exception("User doesn't not exist");
            }
            IEnumerable<Reservation> reservation = reservationRepository.GetReservationsOfUser(userId);
            return _mapper.Map<IEnumerable<ReservationDTO>>(reservation);
        }
    }
}
