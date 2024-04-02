using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.ReservationsDTOS;
using DTO.UserDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.JWT;
using Helpers.StaticFunc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Domain.Concrete
{
    internal class UserDomain : DomainBase, IUserDomain
    {
        public UserDomain(IUnitOfWork unitOfWork,IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
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

		public UserDTO GetUserByEmail(string email)
		{
			User user = userRepository.GetByEmail(email);
			return _mapper.Map<UserDTO>(user);
		}

		public async Task UpdateUserAsync(UserDTO userDTO)
		{
			var receiverIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			Guid userId;
			if (receiverIdClaim != null)
			{
				userId = StaticFunc.ConvertGuid(receiverIdClaim);
			}
			else
			{
				throw new Exception("User doesn't not exist");
			}
			User user = userRepository.GetById(userId);
			user = _mapper.Map<UserDTO, User>(userDTO, user);
			userRepository.Update(user);
			_unitOfWork.Save();
		}

        //
        public int GetActiveUsersCount()
        {
            return userRepository.GetActiveUsersCount();
        }
    }
}