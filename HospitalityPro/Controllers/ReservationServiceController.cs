using DAL.Contracts;
using Domain.Contracts;
using DTO.ReservationServiceDTOs;
using DTO.RoomDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityPro.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationServiceController : ControllerBase
	{
        private readonly IReservationServiceDomain _reservationServiceDomain;
        public ReservationServiceController(IReservationServiceDomain reservationServiceDomain)
        {
			_reservationServiceDomain = reservationServiceDomain;
		}
		[HttpPost]
		public async Task<IActionResult> UpdateRoom(List<ReservationServiceDTO> reservationServiceDTO)
		{
			await _reservationServiceDomain.AddReservationServiceAsync(reservationServiceDTO);
			return NoContent();
		}
	}
}
