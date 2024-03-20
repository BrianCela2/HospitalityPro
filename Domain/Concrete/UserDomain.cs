using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.UserDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.JWT;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Domain.Concrete
{
    internal class UserDomain : DomainBase, IUserDomain
    {
		private readonly JWT _jwt;
		private readonly IUserRolesRepository _rolesRepository;
        public UserDomain(IUnitOfWork unitOfWork,IUserRolesRepository rolesRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor,JWT jwt) : base(unitOfWork, mapper, httpContextAccessor)
        {
			_rolesRepository = rolesRepository;
			_jwt = jwt;
		}

        private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();
        public IList<UserDTO> GetAllUsers()
        {
            IEnumerable<User> user = userRepository.GetAll();
            var test = _mapper.Map<IList<UserDTO>>(user);
            return test;
        }

        public UserDTO GetUserById(Guid id)
        {
            User user = userRepository.GetById(id);
			return _mapper.Map<UserDTO>(user);

		}

		public async Task<string> Login(LoginDTO request)
		{
			var user =  userRepository.GetByEmail(request.Email);

			if ( user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
			{
				throw new ArgumentException("Wrong email or password");
			}
			var userRoles = _rolesRepository.GetUserRolesById(user.UserId);

			var authClaims = new List<Claim>
		    {
			    new(ClaimTypes.Email, user.Email),
		    };
			foreach (var role in userRoles)
			{
				string roleName = ((Helpers.Enumerations.Roles)role.Roles).ToString();
				authClaims.Add(new Claim(ClaimTypes.Role, roleName));
			}

			var token = _jwt.CreateToken(authClaims);
			return token;
		}

		public async Task<User> Register(RegisterDTO request)
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

			return result;

		}


	}
}