using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.ReservationsDTOS;
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
			int DatedifferencesMax = 0;
			int differenceOfDaysRoom = 0;


            foreach (var roomReservation in reservationDto.ReservationRooms){
				var room = roomRepository.GetById(roomReservation.RoomId);
                var roomAvailable = reservationRoomRepository.GetReservationRoomsById(roomReservation.RoomId);

				foreach(var roomDates in roomAvailable){
					if((roomReservation.CheckInDate >= roomDates.CheckInDate && roomReservation.CheckInDate <=roomDates.CheckOutDate)||(roomReservation.CheckOutDate>roomDates.CheckInDate && roomReservation.CheckOutDate<=roomDates.CheckOutDate)){
						throw new Exception("You Reservation can be done because Room is not available");
					}
                     differenceOfDaysRoom = StaticFunc.GetDayDiff(DatedifferencesMax,roomReservation.CheckInDate,roomReservation.CheckOutDate);
					DatedifferencesMax = differenceOfDaysRoom;
                }
                Price += room.Price *differenceOfDaysRoom;
            }
            if (reservationDto.ReservationServices != null)
			{
				foreach (var reservationService in reservationDto.ReservationServices)
				{
					var service = hotelServiceRepository.GetById(reservationService.ServiceId);
					Price += service.Price;
				}
			}
			reservation.TotalPrice = StaticFunc.GetTotalPrice(DatedifferencesMax, Price);
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
			 reservation.TotalPrice +=  service.Price;
            reservationServiceRepository.Add(createReservationService);
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
			Reservation reservation =  reservationRepository.GetById(updateReservationDTO.ReservationId);

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
			if(userId != reservationRepository.GetUserIdByReservation(updateReservationDTO.ReservationId))
			{
				throw new Exception("Wrong user");
			}

			if (reservation == null)
			{
				throw new Exception("Reservation not found");
			}
            int DatedifferencesMax = 0;
            int differenceOfDaysRoom = 0;
			decimal Price = 0;
            foreach (var room in updateReservationDTO.ReservationRooms)
			{
				var roomReservations = reservationRoomRepository.GetReservationRoomsById(room.RoomId);
				foreach(var roomReservation in roomReservations)
				{
					if ((room.CheckInDate < roomReservation.CheckOutDate && room.CheckInDate >= roomReservation.CheckInDate) ||
						(room.CheckOutDate > roomReservation.CheckInDate && room.CheckOutDate <= roomReservation.CheckOutDate))
					{
						throw new Exception($"Room {room.RoomId} is already reserved for the specified dates.");
					}
                    differenceOfDaysRoom = StaticFunc.GetDayDiff(DatedifferencesMax, room.CheckInDate, room.CheckOutDate);
                    DatedifferencesMax = differenceOfDaysRoom;
                }
               Price += roomRepository.GetPriceOfRoom(room.RoomId) * differenceOfDaysRoom;

            }
            reservation = _mapper.Map<Reservation>(updateReservationDTO);
			foreach(var reservationRoom in reservation.ReservationRooms)
			{
				reservationRoom.ReservationId = reservation.ReservationId;
				var room = roomRepository.GetById(reservationRoom.RoomId);
				reservation.TotalPrice += Price;
				reservationRoomRepository.Update(reservationRoom);
			}
			var Reservationservices = reservationServiceRepository.GetReservationServicesByReservationId(reservation.ReservationId);

            foreach (var Reservationservice in Reservationservices )
			{
				var service = hotelServiceRepository.GetById(Reservationservice.ServiceId);
				Price += service.Price;
			}
			reservation.UserId= userId;
			reservation.ReservationStatus = 1;
            reservation.TotalPrice = StaticFunc.GetTotalPrice(DatedifferencesMax, Price);
            reservationRepository.Update(reservation);
			_unitOfWork.Save();

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

		public IEnumerable<ReservationDTO> ReservationsWithRoomService()
		{
            IEnumerable<Reservation> reservations = reservationRepository.ReservationsWithRoomServices();

            var reservationDTO = _mapper.Map<IEnumerable<ReservationDTO>>(reservations);
			return reservationDTO;
        }
    }
}
