using DTO.RoomDTOs;
using DTO.RoomPhotoDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IRoomDomain
    {
        Task AddRoomAsync(CreateRoomDTO createRoomDTO);
        Task<IEnumerable<RoomDTO>> GetAllRoomAsync();
        Task<RoomDTO> GetRoomByIdAsync(Guid id);
        IEnumerable<RoomDTO> GetRoomPhotos();
        Task DeleteRoom(RoomDTO roomDTO); 
        Task UpdateRoom(UpdateRoomDTO updateRoomDTO);

    }
}
