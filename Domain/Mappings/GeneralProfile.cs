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

            CreateMap<RoomPhoto, CreateRoomPhotoDTO>().ReverseMap();
            CreateMap<RoomPhoto, RoomPhotoDTO>().ReverseMap();
            CreateMap<RoomPhoto, UpdateRoomPhotoDTO>().ReverseMap();

            CreateMap<Room,CreateRoomDTO>().ReverseMap();
            CreateMap<Room,RoomDTO>().ReverseMap();
            CreateMap<Room,UpdateRoomDTO>().ReverseMap();

			CreateMap<Reservation, ReservationDTO>().ReverseMap();
			CreateMap<Reservation, CreateReservationDTO>().ReverseMap();
			CreateMap<ReservationRoom, ReservationRoomDTO>().ReverseMap();

            CreateMap<HotelService, HotelServiceDTO>().ReverseMap();
            CreateMap<CreateHotelServiceDTO, HotelService>();
            CreateMap<UpdateHotelServiceDTO, HotelService>();

            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDTO>().ReverseMap();
            CreateMap<Notification, CreateNotificationDTO>().ReverseMap();
        }
        #endregion
    }
}
