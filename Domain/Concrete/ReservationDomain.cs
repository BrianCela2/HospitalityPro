﻿using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using Domain.Notifications;
using DTO.NotificationDTOs;
using DTO.ReservationsDTOS;
using DTO.RoomDTOs;
using Entities.Models;
using Helpers.StaticFunc;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Domain.Concrete
{
    internal class ReservationDomain :DomainBase, IReservationDomain
    {
		private readonly NotificationHub _notificationHub;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
		private readonly PaginationHelper<Reservation> _paginationHelper;
		public ReservationDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,IHubContext<NotificationHub> notificationHubContext,NotificationHub notificationHub) : base(unitOfWork, mapper, httpContextAccessor)
		{
			_notificationHub = notificationHub;
			_notificationHubContext = notificationHubContext;
			_paginationHelper = new PaginationHelper<Reservation>();
		}
        private IReservationRepository reservationRepository => _unitOfWork.GetRepository<IReservationRepository>();
        private IRoomRepository roomRepository => _unitOfWork.GetRepository<IRoomRepository>();
		private IReservationRoomRepository reservationRoomRepository => _unitOfWork.GetRepository<IReservationRoomRepository>();
        private IReservationServiceRepository reservationServiceRepository => _unitOfWork.GetRepository<IReservationServiceRepository>();
		private IHotelServiceRepository hotelServiceRepository => _unitOfWork.GetRepository<IHotelServiceRepository>();
        private INotificationRepository notificationeRepository => _unitOfWork.GetRepository<INotificationRepository>();


        public async Task AddReservationAsync(CreateReservationDTO reservationDto)
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

            var reservation = _mapper.Map<Reservation>(reservationDto);
			reservation.UserId = userId;

			decimal Price = 0;
			int DatedifferencesMax = 0;
			int differenceOfDaysRoom = 0;


            foreach (var roomReservation in reservationDto.ReservationRooms){
				var room = roomRepository.GetById(roomReservation.RoomId);
                var roomAvailable = reservationRoomRepository.GetRoomIncludeReservation(roomReservation.RoomId);

				foreach(var roomDates in roomAvailable){
					if (((roomReservation.CheckInDate >= roomDates.CheckInDate && roomReservation.CheckInDate < roomDates.CheckOutDate) || (roomReservation.CheckOutDate > roomDates.CheckInDate && roomReservation.CheckOutDate <= roomDates.CheckOutDate))&&roomDates.Reservation.ReservationStatus==1)
					{
						throw new Exception("You Reservation can be done because Room is not available");
					}
                }
                differenceOfDaysRoom = StaticFunc.GetDayDiff(DatedifferencesMax, roomReservation.CheckInDate, roomReservation.CheckOutDate);
                DatedifferencesMax = differenceOfDaysRoom;
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
            var notification = new Notification { };
            notification.ReceiverId = (Guid)reservation.UserId;
            notification.MessageContent = " Reservimi u krye me sukses";
            notification.SendDateTime = DateTime.Now;
			notification.IsSeen = false;
            var userConnectionIds = GetConnectionIds();
            if (userConnectionIds.ContainsKey(userId.ToString()))
            {
                var connectionIds = userConnectionIds[userId.ToString()];
                foreach (var connectionId in connectionIds)
                {
                    await _notificationHubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
                }
            }
            else
            {
                throw new Exception("User is not currently connected.");
            }
            notificationeRepository.Add(notification);
            _unitOfWork.Save();
		}
        public Dictionary<string, List<string>> GetConnectionIds()
        {
            return NotificationHub.ConnectedUsers;
        }
        public async Task<IEnumerable<ReservationDTO>> GetAllReservationsAsync(int page, int pageSize, string sortField, string sortOrder)
		{
			IEnumerable<Reservation> reservations = reservationRepository.GetAll();
			IEnumerable<Reservation> paginatedReservations = _paginationHelper.GetPaginatedData(reservations, page, pageSize, sortField, sortOrder);
			return _mapper.Map<IEnumerable<ReservationDTO>>(paginatedReservations);

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
			Reservation reservation = reservationRepository.GetReservation(id);
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
            int DatedifferencesMax = 0;
            int differenceOfDaysRoom = 0;
			decimal Price = 0;
            foreach (var room in updateReservationDTO.ReservationRooms)
			{
				var roomReservations = reservationRoomRepository.GetReservationRoomsByIdExcludingCurrentReservation(room.RoomId,updateReservationDTO.ReservationId);
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
				reservation = _mapper.Map<UpdateReservationDTO, Reservation>(updateReservationDTO, reservation);
			foreach (var reservationRoom in reservation.ReservationRooms)
			{
				reservationRoom.ReservationId = reservation.ReservationId;
				var room = roomRepository.GetById(reservationRoom.RoomId);
				reservation.TotalPrice += Price;
				reservationRoomRepository.Update(reservationRoom);
				_unitOfWork.Save();
			}
			//var Reservationservices = reservationServiceRepository.GetReservationServicesByReservationId(reservation.ReservationId);

            foreach (var Reservationservice in reservation.ReservationServices)
			{
				var service = hotelServiceRepository.GetById(Reservationservice.ServiceId);
				Price += service.Price;
				Reservationservice.ReservationId = reservation.ReservationId;
				reservationServiceRepository.Add(Reservationservice);
				_unitOfWork.Save();
			}
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

		public IEnumerable<ReservationDTO> ReservationsRoomAndService()
		{
            IEnumerable<Reservation> reservations = reservationRepository.ReservationsWithRoomServices();

            var reservationDTO = _mapper.Map<IEnumerable<ReservationDTO>>(reservations);
			return reservationDTO;
        }

		public async Task UpdateReservationStatus(Guid id,int status)
		{
			Reservation reservation = reservationRepository.GetReservation(id);
            if (status == reservation.ReservationStatus)
            {
                throw new Exception("Reservation is in that status already");
            }
            else if (status >= 1 && status <= 2)
            {
				if (status == 1)
				{

					foreach (var roomReservation in reservation.ReservationRooms)
					{
						var room = roomRepository.GetById(roomReservation.RoomId);
						var roomAvailable = reservationRoomRepository.GetRoomIncludeReservation(roomReservation.RoomId);

						foreach (var roomDates in roomAvailable)
						{
							if (((roomReservation.CheckInDate >= roomDates.CheckInDate && roomReservation.CheckInDate < roomDates.CheckOutDate) || (roomReservation.CheckOutDate > roomDates.CheckInDate && roomReservation.CheckOutDate <= roomDates.CheckOutDate)) && roomDates.Reservation.ReservationStatus == 1)
							{
                                var notification = new Notification { };
                                notification.ReceiverId = (Guid)reservation.UserId;
                                notification.MessageContent = "Reservation Status didnt get changed";
                                var userConnectionIds = GetConnectionIds();
                                
                                    var connectionIds = userConnectionIds[notification.ReceiverId.ToString()];
                                    foreach (var connectionId in connectionIds)
                                    {
                                        await _notificationHubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
                                    }
                                
                                throw new Exception("Your Reservation can be done because Room is not available anymore");
							}
						}
					}
				}
                reservation.ReservationStatus = status;
                reservationRepository.Update(reservation);
                _unitOfWork.Save();

            }
            else
            {
                throw new Exception();
            }
		}
	
        public int GetStaysCountWithinDateRange(DateTime startDate, DateTime endDate)
        {
            return reservationRepository.GetStaysCountWithinDateRange(startDate, endDate);
        }

        public decimal GetTotalRevenueWithinDateRange(DateTime startDate, DateTime endDate)
        {
            return reservationRepository.GetTotalRevenueWithinDateRange(startDate, endDate);
        }

		public decimal getTotalReservationPrice(ReservationSampleDTO reservationDto)
		{
			decimal Price = 0;
			int DatedifferencesMax = 0;
			int differenceOfDaysRoom = 0;


			foreach (var roomReservation in reservationDto.ReservationRooms)
			{
				var room = roomRepository.GetById(roomReservation.RoomId);
				differenceOfDaysRoom = StaticFunc.GetDayDiff(DatedifferencesMax, roomReservation.CheckInDate, roomReservation.CheckOutDate);
				DatedifferencesMax = differenceOfDaysRoom;
				Price += room.Price * differenceOfDaysRoom;
			}
			if (reservationDto.ReservationServices != null)
			{
				foreach (var reservationService in reservationDto.ReservationServices)
				{
					var service = hotelServiceRepository.GetById(reservationService.ServiceId);
					Price += service.Price;
				}
			}
			return StaticFunc.GetTotalPrice(DatedifferencesMax, Price);
		}

	}
}
