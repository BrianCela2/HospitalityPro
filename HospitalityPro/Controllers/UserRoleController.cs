using DAL.Contracts;
using Domain.Contracts;
using DTO.RoomDTOs;
using DTO.RoomPhotoDTOs;
using DTO.UserDTO;
using DTO.UserRoles;
using Entities.Models;
using Helpers.Enumerations;
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
				var existingUser =await _userRolesDomain.AddRoleToUser(userRoleDto);
				if (existingUser != null)
				{
					return BadRequest(new { message = "User already has this role." });
				}
				return NoContent();
			}
			else
			{
				return BadRequest();
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("delete")]
		public async Task <IActionResult> DeleteUserRole([FromQuery] Guid userId, [FromQuery] Roles roles)
		{
			try { 
				var userRole = new UserRoleDTO
				{
					UserId = userId,
					Roles = roles
				};
				await _userRolesDomain.RemoveUserRole(userRole);
				return NoContent();
			}
			catch(Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		[Route("getUserRoles")]
		public IActionResult GetAllUserRoles()
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest();
				}

				var userRoles = _userRolesDomain.GetUserRolesAsync();

				if (userRoles != null)
				{
					return Ok(userRoles);
				}
				else
				{
					return NotFound();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex);
			}
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		[Route("getUserRoleDetails")]
		public async Task<IActionResult>  GetAllUserRoleDetails([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortField = "FirstName", [FromQuery] string sortOrder = "asc", [FromQuery] string searchString = null)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest();
				}

				var userRoles = await _userRolesDomain.GetUserRoleDetailsAsync(page, pageSize, sortField, sortOrder, searchString);

				if (userRoles != null)
				{
					return Ok(userRoles);
				}
				else
				{
					return NotFound();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex);
			}
		}

	}
}
