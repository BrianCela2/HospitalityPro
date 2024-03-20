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
		private readonly  IAuthDomain _authDomain;

		public AuthController(IAuthDomain authDomain)
		{
			_authDomain = authDomain;
		}
		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO request)
		{
			if (String.IsNullOrEmpty(request.FirstName)
				|| String.IsNullOrEmpty(request.LastName)
				|| String.IsNullOrEmpty(request.Password)
				|| String.IsNullOrEmpty(request.Email)
				|| String.IsNullOrEmpty(request.PhoneNumber)
				|| String.IsNullOrEmpty(request.City)
				|| String.IsNullOrEmpty(request.Country))
			{
				return BadRequest(new { message = "All fields are required" });
			}

			var response = await _authDomain.Register(request);

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
			var response = await _authDomain.Login(request);

			if (response != null)
			{
				return Ok(response);
			}

			return BadRequest(new { message = "User login unsuccessful" });
		}

	}
}
