using DAL.Contracts;
using Domain.Contracts;
using DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityPro.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserDomain _userDomain;
		private readonly IUserRolesRepository _rolesRepository;

		public AuthController(IUserDomain userDomain, IUserRolesRepository rolesRepository)
		{
			_userDomain = userDomain;
			_rolesRepository = rolesRepository;
		}
		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO request)
		{
			if (String.IsNullOrEmpty(request.FirstName))
			{
				return BadRequest(new { message = "First name needs to be entered" });
			}
			else if (String.IsNullOrEmpty(request.LastName))
			{
				return BadRequest(new { message = "Last name needs to be entered" });
			}
			else if (String.IsNullOrEmpty(request.Password))
			{
				return BadRequest(new { message = "Password needs to be entered" });
			}
			else if (String.IsNullOrEmpty(request.Email))
			{
				return BadRequest(new { message = "Email needs to be entered" });
			}
			else if (String.IsNullOrEmpty(request.PhoneNumber))
			{
				return BadRequest(new { message = "Phone Number needs to be entered" });
			}
			else if (String.IsNullOrEmpty(request.City))
			{
				return BadRequest(new { message = "City needs to be entered" });
			}
			else if (String.IsNullOrEmpty(request.Country))
			{
				return BadRequest(new { message = "Country needs to entered" });
			}

			var response = await _userDomain.Register(request);

			if (response != null)
			{
				return Ok(response);
			}

			return BadRequest(new { message = "User registration unsuccessful" });
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO request)
		{
			if (String.IsNullOrEmpty(request.Email))
			{
				return BadRequest(new { message = "Email needs to be entered" });
			}
			else if (String.IsNullOrEmpty(request.Password))
			{
				return BadRequest(new { message = "Password needs to be entered" });
			}
			var response = await _userDomain.Login(request);

			if (response != null)
			{
				return Ok(response);
			}

			return BadRequest(new { message = "User login unsuccessful" });
		}

	}
}
