using DAL.Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
	internal class ReservationRoomRepository:BaseRepository<ReservationRoom, Guid>, IReservationRoomRepository
		{

		public ReservationRoomRepository(HospitalityProContext dbContext) : base(dbContext)
		{
		}

		public IEnumerable<ReservationRoom> GetReservationRoomsById(Guid roomId)
		{
			return context.Where(r=> r.RoomId == roomId).ToList();
		}
	}
}
