using Domain.Contracts;
using DTO.ReservationRoomDTOs;
using DTO.RoomDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityPro.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationRoomController : ControllerBase
	{
        private readonly IReservationRoomDomain _reservationRoomDomain;
        public ReservationRoomController(IReservationRoomDomain reservationRoomDomain)
        {
			_reservationRoomDomain = reservationRoomDomain;
		}
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ReservationRoomDTO>))]
		public async Task<IActionResult> GetAll()
		{
			var reservationRooms = await _reservationRoomDomain.GetAllReservationRoomsAsync();
			if (reservationRooms == null) { return NotFound(); }
			return Ok(reservationRooms);
		}

		[HttpGet("{roomId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ReservationRoomDTO>))]
		public async Task<IActionResult> GetById(Guid roomId)
		{
			var reservationRoom = await _reservationRoomDomain.GetReservationsRoomByRoomId(roomId);
			if (reservationRoom == null) { return NotFound(); }
			return Ok(reservationRoom);
		}
	}
}
