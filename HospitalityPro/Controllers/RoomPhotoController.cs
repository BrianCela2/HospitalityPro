using Microsoft.AspNetCore.Mvc;
using DTO.RoomPhotoDTOs;
using Domain.Contracts;

namespace HospitalityPro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomPhotoController : ControllerBase
    {
        private readonly IRoomPhotoDomain _roomPhotoDomain;
        public RoomPhotoController(IRoomPhotoDomain roomPhotoDomain)
        {
            _roomPhotoDomain = roomPhotoDomain;
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoAsync([FromForm] CreateRoomPhotoDTO createRoomPhotoDTO)
        {
            if (ModelState.IsValid)
            {
                await _roomPhotoDomain.AddPhotoAsync(createRoomPhotoDTO);
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(RoomPhotoDTO))]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (ModelState.IsValid)
            {
                var roomPhoto = await _roomPhotoDomain.GetPhotoByIdAsync(id);
                if (roomPhoto == null) { return NotFound(); }
                return Ok(roomPhoto);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomPhoto(Guid id)
        {
            if (ModelState.IsValid)
            {
                var room = await _roomPhotoDomain.GetPhotoByIdAsync(id);
                if (room == null) { return NotFound(); }

                await _roomPhotoDomain.DeletePhotoAsync(id);
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
