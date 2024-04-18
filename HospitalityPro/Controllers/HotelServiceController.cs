using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Contracts;
using DTO.HotelServiceDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/hotelservices")]
    [ApiController]
    public class HotelServiceController : ControllerBase
    {
        private readonly IHotelServiceDomain _hotelServiceDomain;

        public HotelServiceController(IHotelServiceDomain hotelServiceDomain)
        {
            _hotelServiceDomain = hotelServiceDomain;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelServiceDTO>>> GetAllHotelServicesAsync()
        {
            try
            {
                var hotelServices = await _hotelServiceDomain.GetAllHotelServicesAsync();
                return Ok(hotelServices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelServiceDTO>> GetHotelServiceByIdAsync(Guid id)
        {
            try
            {
                var hotelService = await _hotelServiceDomain.GetHotelServiceByIdAsync(id);
                if (hotelService == null)
                {
                    return NotFound($"Hotel service with ID {id} not found");
                }
                return Ok(hotelService);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddHotelServiceAsync([FromBody] CreateHotelServiceDTO hotelServiceDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _hotelServiceDomain.AddHotelServiceAsync(hotelServiceDTO);
                return StatusCode(201); // Created
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHotelServiceAsync(Guid id, [FromBody] UpdateHotelServiceDTO hotelServiceDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != hotelServiceDTO.ServiceID)
                {
                    return BadRequest("Service ID mismatch");
                }

                await _hotelServiceDomain.UpdateHotelServiceAsync(hotelServiceDTO);
                return NoContent(); // Updated successfully
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotelServiceAsync(Guid id)
        {
            try
            {
                var hotelService = await _hotelServiceDomain.GetHotelServiceByIdAsync(id);
                if (hotelService == null)
                {
                    return NotFound($"Hotel service with ID {id} not found");
                }

                await _hotelServiceDomain.DeleteHotelServiceAsync(id);
                return NoContent(); // Deleted successfully
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("ServicesOfReservation/{id}")]
        public ActionResult<IEnumerable<HotelServiceDTO>> GetServicesReservations(Guid id)
        {
            try
            {
                var hotelServices = _hotelServiceDomain.GetServiceReservation(id);
                return Ok(hotelServices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
