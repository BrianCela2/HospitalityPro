using DAL.Contracts;
using Domain.Contracts;
using DTO.UserDTO;
using Helpers.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HospitalityPro.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly  IAuthDomain _authDomain;
		private readonly JWT _jwt;

		public AuthController(IAuthDomain authDomain, IConfiguration configuration)
		{
			_authDomain = authDomain;
			_jwt = new JWT(configuration);
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

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] string expiredToken)
		{
			try
			{
				var newToken = _jwt.RefreshToken(expiredToken);
				return Ok(newToken);
			}
			catch (SecurityTokenException ex)
			{
				// Handle token validation errors
				return BadRequest("Token validation failed.");
			}
			catch (Exception ex)
			{
				// Handle other errors
				return StatusCode(500, "Internal server error.");
			}
		}

		}
}
