using DAL.Contracts;
using Entities.Models;
using Entities.SearchParametersList;
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

		public List<List<Room>> GetRoomsAvailable(List<SearchParameters> searchParameters)
		{
			List<List<Room>> availableRoomsList = new List<List<Room>>();

			foreach (var criteria in searchParameters)
			{
				var availableRooms = context
					.Where(room => room.Capacity >= criteria.Capacity &&
								   !room.ReservationRooms.Any(reservation =>
										!(criteria.CheckOutDate <= reservation.CheckInDate ||
										  criteria.CheckInDate >= reservation.CheckOutDate)))
					.Include(x=>x.RoomPhotos)
					.ToList();
				availableRoomsList.Add(availableRooms);
			}
			return availableRoomsList;
		}
	}
}
