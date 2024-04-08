using DAL.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    internal class RoomRepository : BaseRepository<Room, Guid>, IRoomRepository
    {

        public RoomRepository(HospitalityProContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Room> GetAllRoomsPhoto()
        {
            var rooms = context.Include(x => x.RoomPhotos).ToList();
            return rooms;
        }
        public Room GetRoomWithPhotos(Guid roomId)
        {
            var room = context.Include(x => x.RoomPhotos).Where(x=>x.RoomId==roomId).FirstOrDefault();
            return room;
        }
        public Room GetRoomReservations(Guid roomId)
        {
           var room = context.Where(x => x.RoomId == roomId).Include(x => x.ReservationRooms).FirstOrDefault();
            return room;
        }

        public decimal GetPriceOfRoom(Guid roomId)
        {
            var price = context.Where(x=>x.RoomId ==roomId).Select(x=> x.Price).FirstOrDefault();
            return price;
        }

         
        public int GetAvailableRoomsCount()
        {
            return context.Count();
        }

    }
}
