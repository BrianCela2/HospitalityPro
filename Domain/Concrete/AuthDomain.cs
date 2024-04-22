using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.UserDTO;
using Entities.Models;
using Helpers.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
	internal class AuthDomain : DomainBase, IAuthDomain
	{

		private readonly JWT _jwt;

		public AuthDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
			: base(unitOfWork, mapper, httpContextAccessor)
		{
			_jwt = new JWT(configuration);
		}

		private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();
		private IUserRolesRepository userRolesRepository => _unitOfWork.GetRepository<IUserRolesRepository>();

		public async Task<string> Login(LoginDTO request)
		{
			var user = userRepository.GetByEmail(request.Email);

			if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
			{
				throw new Exception("Wrong email or password");
			}
			var userRoles = userRolesRepository.GetUserRolesById(user.UserId);

			var authClaims = new List<Claim>
			{
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.NameIdentifier, user.UserId.ToString())
			};
			foreach (var role in userRoles)
			{
				string roleName = ((Helpers.Enumerations.Roles)role.Roles).ToString();
				authClaims.Add(new Claim(ClaimTypes.Role, roleName));
			}

			var token = _jwt.CreateToken(authClaims);
			return token;
		}

		public async Task Register(RegisterDTO request)
		{
			var userByEmail = userRepository.Find(u => u.Email == request.Email);

			if (userByEmail.Any())
			{
				throw new Exception("User already exists.");
			}
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

			var userMap = _mapper.Map<User>(request);

			userMap.Password = passwordHash;

			var result = userRepository.Add(userMap);

			 _unitOfWork.Save();

		}
	}
}
