using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.JWT
{
	public class JWT
	{
		private readonly IConfiguration _configuration;
        public JWT(IConfiguration configuration)
        {
			_configuration = configuration;
		}

		public string CreateToken(IEnumerable<Claim> authClaims)
		{

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_configuration.GetSection("AppSettings:Token").Value!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(authClaims),
				IssuedAt = DateTime.UtcNow,
				Issuer = _configuration["JWT:Issuer"],
				Audience = _configuration["JWT:Audience"],
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var jwt = tokenHandler.WriteToken(token);
			return jwt;
		}
		public string RefreshToken(string expiredToken)
		{
			var tokenValidationParams = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)),
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = false // Don't validate token expiration here
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;

			// Validate the expired token without checking expiration
			var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParams, out securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			// Retrieve claims from the expired token
			var emailClaim = principal.FindFirst(ClaimTypes.Email);
			var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
			var roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value);


			// Create new claims for the refreshed token
			var newAuthClaims = new List<Claim>
		{
			new Claim(ClaimTypes.Email, emailClaim.Value),
			new Claim(ClaimTypes.NameIdentifier, userIdClaim.Value),
            // Add other claims as needed
        };
			foreach (var role in roles)
			{
				newAuthClaims.Add(new Claim(ClaimTypes.Role, role)); // Assuming each role is a string
			}

			// Create a new token with refreshed expiration
			var newToken = CreateToken(newAuthClaims);

			return newToken;
		}
        public static string GetUserIdFromToken(string jwtToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);

                var userIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting user ID from JWT token: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
