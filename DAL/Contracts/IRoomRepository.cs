using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IRoomRepository : IRepository<Room, Guid>
    {
        IEnumerable<Room> GetAllRoomsPhoto();
        Room GetRoomReservations(Guid roomId);
        decimal GetPriceOfRoom(Guid roomId);
        Room GetRoomWithPhotos(Guid roomId);
        public int GetAvailableRoomsCount();
    }
}
