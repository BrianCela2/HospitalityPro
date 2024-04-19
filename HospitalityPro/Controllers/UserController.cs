using DAL.Contracts;
using Domain.Contracts;
using DTO.UserDTO;
using Entities.Models;
using LamarCodeGeneration.Frames;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;

namespace HospitalityPro.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserDomain _userDomain;

        public UserController(IUserDomain userDomain)
        {
            _userDomain = userDomain;
        }

        [Authorize]
		[HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page=1, [FromQuery] int pageSize=10, [FromQuery] string sortField = "FirstName", [FromQuery] string sortOrder="asc", [FromQuery] string searchString=null )
        {
            try
            {
                if (!ModelState.IsValid)
				{
					return BadRequest();
				}
				var users = await _userDomain.GetAllUsers(page,pageSize,sortField,sortOrder,searchString);
				if (users != null)
				{
                    return Ok(users);
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
        [Route("{userId}")]
        public IActionResult GetUserById([FromRoute] Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                var user = _userDomain.GetUserById(userId);

                if (user != null)
                    return Ok(user);

                return NotFound();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

		[HttpPut]
		public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO)
		{
			try
			{
				await _userDomain.UpdateUserAsync(userDTO);
				return Ok("User updated successfully");
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex);
			}
		}

	}
}

