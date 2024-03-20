using Domain.Contracts;
using DTO.RoomDTOs;
using DTO.RoomPhotoDTOs;
using DTO.UserDTO;
using DTO.UserRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityPro.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserRoleController : ControllerBase
	{
		private readonly IUserRolesDomain _userRolesDomain;

        public UserRoleController(IUserRolesDomain userRolesDomain)
        {
			_userRolesDomain = userRolesDomain;
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("{userId}")]
		public async Task<IActionResult> GetUserRolesById(Guid userId)
		{
			var userRoles = await _userRolesDomain.GetUserRoleById(userId);
			if (userRoles == null) { return NotFound(); }
			return Ok(userRoles);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> AddRoleToUser(UserRoleDTO userRoleDto)
		{
			if (ModelState.IsValid)
			{
				await _userRolesDomain.AddRoleToUser(userRoleDto);
				return NoContent();
			}
			else
			{
				return BadRequest();
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete]
		public async Task<IActionResult> DeleteUserRole(Guid userId, int role)
		{
			if (ModelState.IsValid)
			{
				await _userRolesDomain.RemoveUserRole(userId,role);
				return NoContent();
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
