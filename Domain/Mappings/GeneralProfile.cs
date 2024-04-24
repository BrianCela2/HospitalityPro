using AutoMapper;
using DTO.ReservationRoomDTOs;
using DTO.ReservationsDTOS;
using DTO.HotelServiceDTOs;
using DTO.NotificationDTOs;
using DTO.RoomDTOs;
using DTO.RoomPhotoDTOs;
using DTO.UserDTO;
using DTO.UserRoles;
using Entities.Models;
using DTO.ReservationServiceDTOs;
using DTO.UserRoleDTO;
using DTO.UserHistoryDTOs;

namespace Domain.Mappings
{
    public class GeneralProfile : Profile
    {
        #region User
        public GeneralProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
            CreateMap<UserRole, UserRoleDTO>().ReverseMap();
			CreateMap<UserRole, UserRoleDetailDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();

			CreateMap<RoomPhoto, CreateRoomPhotoDTO>().ReverseMap();
            CreateMap<RoomPhoto, RoomPhotoDTO>().ReverseMap();
            CreateMap<RoomPhoto, UpdateRoomPhotoDTO>().ReverseMap();

            CreateMap<Room,CreateRoomDTO>().ReverseMap();
            CreateMap<Room,RoomDTO>().ReverseMap();
            CreateMap<Room,UpdateRoomDTO>().ReverseMap();

			CreateMap<Reservation, ReservationDTO>().ReverseMap();
			CreateMap<Reservation, CreateReservationDTO>().ReverseMap();
			CreateMap<Reservation, UpdateReservationDTO>().ReverseMap();
			CreateMap<Reservation, ReservationSampleDTO>().ReverseMap();

			CreateMap<ReservationRoom, ReservationRoomDTO>().ReverseMap();
            CreateMap<ReservationRoom, ReservationRoomDetailsDTO>().ReverseMap();


            CreateMap<HotelService, HotelServiceDTO>().ReverseMap();
            CreateMap<CreateHotelServiceDTO, HotelService>();
            CreateMap<UpdateHotelServiceDTO, HotelService>();

            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDTO>().ReverseMap();
            CreateMap<Notification, CreateNotificationDTO>().ReverseMap();

            CreateMap<ReservationService,CreateReservationServiceDTO>().ReverseMap();
            CreateMap<ReservationService,ReservationServiceDTO>().ReverseMap();
            CreateMap<ReservationDTO, UpdateReservationDTO>().ReverseMap();
            CreateMap<Reservation, UpdateReservationStatusDTO>().ReverseMap();

            CreateMap<UserHistory, UserHistoryDTO>().ReverseMap();

        }
        #endregion
    }
}
