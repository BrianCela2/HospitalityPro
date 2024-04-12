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
			
			try
			{
				await _authDomain.Register(request);
				return Ok(new { message = "User registration successful" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
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
