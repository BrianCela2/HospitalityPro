using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
	public interface IReservationRoomRepository : IRepository<ReservationRoom, Guid>
	{
		IEnumerable<ReservationRoom> GetReservationRoomsById(Guid roomId);	
	}
}
