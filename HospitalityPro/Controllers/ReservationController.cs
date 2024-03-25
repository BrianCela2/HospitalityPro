using Domain.Contracts;
using DTO.ReservationsDTOS;
using DTO.RoomDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityPro.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationController : ControllerBase
	{
        private readonly IReservationDomain _reservationDomain;
        public ReservationController(IReservationDomain reservationDomain)
        {
			_reservationDomain = reservationDomain;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllReservations()
		{
			var reservations =  await _reservationDomain.GetAllReservationsAsync();
			if (reservations == null) { return NotFound(); }
			return Ok(reservations);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(ReservationDTO))]
		public async Task<IActionResult> GetReservationById(Guid id)
		{
			var reservation = await _reservationDomain.GetReservationByIdAsync(id);
			if (reservation == null) { return NotFound(); }
			return Ok(reservation);
		}

		[HttpPost]
		public async Task<IActionResult> AddReservation([FromForm] CreateReservationDTO reservationDTO)
		{
			if (reservationDTO == null) { return NotFound(); }
			await _reservationDomain.AddReservationAsync(reservationDTO);
			return NoContent();
		}
	}
}
