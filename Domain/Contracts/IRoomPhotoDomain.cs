using DTO.RoomDTOs;
using DTO.RoomPhotoDTOs;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IRoomPhotoDomain
    {
        Task AddPhotoAsync(CreateRoomPhotoDTO createRoomDTO);
        Task<RoomPhotoDTO> GetPhotoByIdAsync(Guid id);
        Task DeletePhotoAsync(Guid id);
        Task UpdatePhoto(UpdateRoomPhotoDTO updateRoomPhotoDTO);
    }
}
