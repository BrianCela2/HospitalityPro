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
	internal class ReservationRoomRepository:BaseRepository<ReservationRoom, Guid>, IReservationRoomRepository
	{

		public ReservationRoomRepository(HospitalityProContext dbContext) : base(dbContext)
		{
		}

		public IEnumerable<ReservationRoom> GetReservationRoomsById(Guid roomId)
		{
			return context.Include(x=>x.Room).Where(r=> r.RoomId == roomId).ToList();
		}
        public IEnumerable<ReservationRoom> GetReservationByRoom(Guid roomId)
        {
            return context.Where(r => r.RoomId == roomId).ToList();
        }

        public IEnumerable<ReservationRoom> GetRoomsByReservationId(Guid reservationId)
        {
            var reservationRoom = context.Include(x => x.Room).Where(x => x.ReservationId == reservationId).ToList();
            return reservationRoom;
        }

		public IEnumerable<ReservationRoom> GetReservationRoomsByIdExcludingCurrentReservation(Guid roomId, Guid reservationIdToExclude)
		{
			return context
				.Where(rr => rr.RoomId == roomId && rr.ReservationId != reservationIdToExclude)
				.ToList();
		}

   
        public int GetRoomOccupancyWithinDateRange(Guid roomId, DateTime startDate, DateTime endDate)
        {
            return context.Count(r => r.RoomId == roomId && r.CheckInDate >= startDate && r.CheckOutDate <= endDate);
        }

        public IEnumerable<ReservationRoom> GetRoomReservationsWithinDateRange(DateTime startDate, DateTime endDate)
        {
            return context.Where(r => r.CheckInDate >= startDate && r.CheckOutDate <= endDate).ToList();
        }
        public IEnumerable<ReservationRoom> GetRoomReservation(Guid reservationId)
        {
            var reservationrooms = context
                .Include(x => x.Room)
                .Where(x => x.ReservationId==reservationId)
                .ToList();
            return reservationrooms;
        }
    }
}
