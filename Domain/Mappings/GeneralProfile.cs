using AutoMapper;
using DTO.RoomDTOs;
using DTO.RoomPhotoDTOs;
using DTO.UserDTO;
using Entities.Models;

namespace Domain.Mappings
{
    public class GeneralProfile : Profile
    {
        #region User
        public GeneralProfile()
        {
           CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<RoomPhoto, CreateRoomPhotoDTO>().ReverseMap();
            CreateMap<RoomPhoto, RoomPhotoDTO>().ReverseMap();
            CreateMap<Room,CreateRoomDTO>().ReverseMap();
            CreateMap<Room,RoomDTO>().ReverseMap();
            CreateMap<RoomPhoto, UpdateRoomPhotoDTO>().ReverseMap();

        }

        #endregion
    }
}
