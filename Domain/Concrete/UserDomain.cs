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
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Domain.Concrete
{
    internal class UserDomain : DomainBase, IUserDomain
    {
		private readonly PaginationHelper<User> _paginationHelper;
		public UserDomain(IUnitOfWork unitOfWork,IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
			_paginationHelper = new PaginationHelper<User>();
		}

        private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();

		public async Task<PaginatedUserDto> GetAllUsers(int page, int pageSize, string sortField, string sortOrder, string searchString)
		{
			searchString = searchString?.ToLower();
			IEnumerable<User> users = userRepository.GetAll();
			Func<User, bool> filterFunc = u => string.IsNullOrEmpty(searchString) || u.FirstName.ToLower().Contains(searchString) || u.Email.Contains(searchString);
			IEnumerable<User> paginatedUsers = _paginationHelper.GetPaginatedData(users, page, pageSize, sortField, sortOrder,searchString, filterFunc: filterFunc);
			var allUsers = _mapper.Map<IEnumerable<UserDTO>>(paginatedUsers);
			var totalUsersCount = users.Count();
			var totalPages = (int)Math.Ceiling((double)totalUsersCount / pageSize);
			return new PaginatedUserDto
			{
				Users = allUsers,
				TotalPages = totalPages
			};
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