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

        // 
        public int GetAvailableRoomsCount()
        {
            return context.Count();
        }

    }
}
