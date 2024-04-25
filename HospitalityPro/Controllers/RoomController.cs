using Domain.Contracts;
using DTO.RoomDTOs;
using DTO.SearchParametersList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityPro.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomDomain _roomDomain;

        public RoomController(IRoomDomain roomDomain)
        {
            _roomDomain = roomDomain;
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromForm] CreateRoomDTO createRoomDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (createRoomDTO == null)
                {
                    return NotFound();
                }
                await _roomDomain.AddRoomAsync(createRoomDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }     
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoomDTO>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if(!ModelState.IsValid) { 
                    return BadRequest();
                }
                var rooms = await _roomDomain.GetAllRoomAsync();
                if (rooms == null) { 
                    return NotFound(); 
                }
                return Ok(rooms);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
          
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(RoomDTO))]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var room = await _roomDomain.GetRoomByIdAsync(id);
                if (room == null)
                {
                    return NotFound();
                }
                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(RoomDTO))]
        public IActionResult GetRoom(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var room =  _roomDomain.GetRoomWithPhoto(id);
                if (room == null)
                {
                    return NotFound();
                }
                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetRoomPhotos([FromQuery] int page = 1, [FromQuery] int pageSize = 9, [FromQuery] string sortField = "RoomNumber", [FromQuery] string sortOrder = "asc")
        {
            var rooms = await _roomDomain.GetRoomPhotos(page, pageSize, sortField, sortOrder);
            if (rooms == null) { return NotFound(); }
            return Ok(rooms);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            if (ModelState.IsValid)
            {
                var room = await _roomDomain.GetRoomByIdAsync(id);
                if (room == null) { return NotFound(); }

                await _roomDomain.DeleteRoom(room);
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomDTO updateRoomDto)
        {
            if (id != updateRoomDto.RoomId) { return NotFound(); }
            await _roomDomain.UpdateRoom(updateRoomDto);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id,[FromQuery]int status)
        {
            var room = await _roomDomain.GetRoomByIdAsync(id);
            if (id != room.RoomId) { return NotFound(); }
            await _roomDomain.UpdateRoomStatus(status, room);
            return NoContent();
        }

        [HttpPost]
        public IActionResult SearchRooms(List<SearchParameters> searchParameters)
        {
            var roomLists = _roomDomain.GetRoomsAvailable(searchParameters);
            return Ok(roomLists);
        }
	}
}
