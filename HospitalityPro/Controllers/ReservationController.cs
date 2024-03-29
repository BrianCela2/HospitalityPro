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
			var reservations = await _reservationDomain.GetAllReservationsAsync();
			if (reservations == null)
			{ return NotFound(); }
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
		public async Task<IActionResult> AddReservation([FromBody] CreateReservationDTO reservationDTO)
		{
			if (reservationDTO == null) { return NotFound(); }
			await _reservationDomain.AddReservationAsync(reservationDTO);
			return NoContent();
		}
		[HttpPut("{reservationID}/{serviceID}")]
        public async Task<IActionResult> ExtraService(Guid reservationID,Guid serviceID)
        {
			ReservationDTO reservationDTO = await _reservationDomain.GetReservationByIdAsync(reservationID);
            if (reservationDTO == null) { return NotFound(); }
			await _reservationDomain.AddExtraService(reservationID, serviceID);
            return NoContent();
        }
        [HttpDelete("{reservationId}")]
        public async Task<IActionResult> DeleteReservation(Guid reservationId)
        {
            if (ModelState.IsValid)
            {
                var reservation = await _reservationDomain.GetReservationByIdAsync(reservationId);
                if (reservation == null) { return NotFound(); }

                await _reservationDomain.DeleteReservation(reservationId);
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
		[HttpGet]
		[Route("GetReservationForUser")]
		public IActionResult GetReservationForUser()
		{
            var reservations =  _reservationDomain.GetReservationsOfUser();
            if (reservations == null)
            { return NotFound(); }
            return Ok(reservations);
        }
        [HttpGet]
        [Route("GetReservationsWithRooms")]
        public IActionResult GetReservationWithRooms()
        {
            var reservations = _reservationDomain.ReservationsWithRoomService();
            if (reservations == null)
            { return NotFound(); }
            return Ok(reservations);
        }

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateReservation(Guid id, UpdateReservationDTO updateReservationDto)
		{
			if (id != updateReservationDto.ReservationId) { return NotFound(); }
			await _reservationDomain.UpdateReservation(updateReservationDto);
			return NoContent();
		}
	}
	
}
