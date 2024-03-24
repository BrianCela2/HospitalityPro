using AutoMapper;
using DTO.ReservationsDTOS;
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
            CreateMap<Room,CreateRoomDTO>().ReverseMap();
            CreateMap<Room,RoomDTO>().ReverseMap();
            CreateMap<RoomPhoto, UpdateRoomPhotoDTO>().ReverseMap();
            CreateMap<Room,UpdateRoomDTO>().ReverseMap();

			CreateMap<Reservation, ReservationDTO>().ReverseMap();
		}
        #endregion
    }
}
