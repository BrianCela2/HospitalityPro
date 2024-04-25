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

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<HotelServiceDTO>>> GetAllHotelServicesAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortField = "Price", [FromQuery] string sortOrder = "asc", [FromQuery] string searchString = null)
        {
            try
            {
                var hotelServices = await _hotelServiceDomain.GetAllHotelServicesAsync(page, pageSize, sortField, sortOrder, searchString);
                return Ok(hotelServices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetHotelServiceById")]
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

        [HttpPost("add")]
        public async Task<ActionResult> AddHotelServiceAsync([FromBody] CreateHotelServiceDTO hotelServiceDTO)
        {
            try
            {
                if (hotelServiceDTO?.Price < 0)
                {
                    ModelState.AddModelError("Price", "Price must be a positive number");
                    return BadRequest(ModelState);
                }

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

        [HttpPut("{id}", Name = "UpdateHotelService")]
        public async Task<ActionResult> UpdateHotelServiceAsync(Guid id, [FromBody] UpdateHotelServiceDTO hotelServiceDTO)
        {
            try
            {
                if (hotelServiceDTO == null)
                {
                    return BadRequest("Hotel service data is required");
                }

                if (id != hotelServiceDTO.ServiceID)
                {
                    return BadRequest("Service ID mismatch");
                }

                if (hotelServiceDTO.Price < 0)
                {
                    ModelState.AddModelError("Price", "Price must be a positive number");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _hotelServiceDomain.UpdateHotelServiceAsync(hotelServiceDTO);
                return NoContent(); // Updated successfully
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}", Name = "DeleteHotelService")]
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

        [HttpGet("services/reservations/{id}", Name = "GetServicesReservations")]
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
